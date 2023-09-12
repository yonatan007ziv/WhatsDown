using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using WhatsDown.Core.Components;
using WhatsDown.Core.Configuration;
using WhatsDown.Core.Exceptions;
using WhatsDown.Core.Interfaces;
using WhatsDown.WPF.Interfaces;

namespace WhatsDown.WPF.Networking.Tcp;

class TcpClientCommunicationHandler : INetworkClient
{
    private readonly ILogger logger;

    private readonly TcpClient socket;
    private readonly EncryptionHandler encryption;
    private readonly TaskCompletionSource<bool> encryptionReady;
    private readonly CancellationTokenSource disconnectedCts;
    private NetworkStream Ns { get => socket.GetStream(); }

    public TcpClientCommunicationHandler(ILogger logger)
    {
        this.logger = logger;

        socket = new TcpClient();
        encryption = new EncryptionHandler();
        encryptionReady = new TaskCompletionSource<bool>();
        disconnectedCts = new CancellationTokenSource();
    }


    /// <exception cref="DisconnectedFromEndPointException"/>
    public async Task<bool> Connect(IPAddress addr, int port)
    {
        try
        {
            await socket.ConnectAsync(addr, port);
            if (!await EstablishEncryption())
                throw new EncryptionFailedException();
            return true;
        }
        catch (DisconnectedFromEndPointException ex) { logger.LogError($"Disconnected: {ex.Message}"); }
        catch (EncryptionFailedException ex) { logger.LogError($"Encryption Failed: {ex.Message}"); }
        catch (SocketException ex) { logger.LogError($"Connection Failed: {ex.Message}"); }
        return false;
    }

    public void Disconnect()
    {
        socket.Close();
        socket.Dispose();
    }

    public async Task WriteMessage(string message)
    {
        await encryptionReady.Task;

        byte[] writeBuffer = EncodingConfiguration.EncodingScheme.GetBytes(message);
        await WriteBytes(writeBuffer);
        disconnectedCts.Cancel();
    }

    public async Task<string> ReadMessage()
    {
        await encryptionReady.Task;

        byte[] readBuffer;
        readBuffer = await ReadBytes();
        return EncodingConfiguration.EncodingScheme.GetString(readBuffer);
    }

    private async Task<bool> EstablishEncryption()
    {
        try
        {
            // Send Rsa Details
            byte[] rsaPublicKey = encryption.ExportRsa();
            await WriteBytes(rsaPublicKey);

            // Import Aes Details
            byte[] encryptedAesPrivateKey = await ReadBytes();
            byte[] aesPrivateKey = encryption.DecryptRsa(encryptedAesPrivateKey);
            encryption.ImportAesPrivateKey(aesPrivateKey);
            byte[] encryptedAesIv = await ReadBytes();
            byte[] aesIv = encryption.DecryptRsa(encryptedAesIv);
            encryption.ImportAesIv(aesIv);

            // Test Encryption: Send
            string msgTest = "SUCCESS";
            byte[] decryptedTest = EncodingConfiguration.EncodingScheme.GetBytes(msgTest);
            byte[] encryptedTest = encryption.EncryptAes(decryptedTest);
            await WriteBytes(encryptedTest);

            // Test Encryption: Receive
            encryptedTest = await ReadBytes();
            decryptedTest = encryption.DecryptAes(encryptedTest);
            msgTest = EncodingConfiguration.EncodingScheme.GetString(decryptedTest);

            return msgTest == "SUCCESS";
        }
        catch { return false; }
    }

    private async Task WriteBytes(byte[] bytes)
    {
        try
        {
            byte[] prefixedBuffer = PrefixLength(bytes);
            await Ns.WriteAsync(prefixedBuffer, disconnectedCts.Token);
        }
        catch
        {
            throw new DisconnectedFromEndPointException();
        }
    }

    private async Task<byte[]> ReadBytes()
    {
        try
        {
            byte[] lengthBuffer = new byte[4];
            await Ns.ReadAsync(lengthBuffer, disconnectedCts.Token);

            int length = ExtractLength(lengthBuffer);
            byte[] readBufer = new byte[length];
            await Ns.ReadAsync(readBufer, disconnectedCts.Token);

            return readBufer;
        }
        catch
        {
            throw new DisconnectedFromEndPointException();
        }
    }

    private static int ExtractLength(byte[] bytes)
    {
        return BitConverter.ToInt32(bytes);
    }

    private static byte[] PrefixLength(byte[] bytes)
    {
        byte[] length = BitConverter.GetBytes(bytes.Length);
        byte[] prefixed = new byte[bytes.Length + sizeof(int)];

        Array.Copy(length, 0, prefixed, 0, 4);
        Array.Copy(bytes, 0, prefixed, 4, bytes.Length);

        return prefixed;
    }

    public void Dispose()
    {
        Disconnect();
    }
}