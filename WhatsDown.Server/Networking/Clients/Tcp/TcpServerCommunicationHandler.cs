using Microsoft.Extensions.DependencyInjection;
using System.Net.Sockets;
using WhatsDown.Core.Components;
using WhatsDown.Core.Configuration;
using WhatsDown.Core.Exceptions;
using WhatsDown.Core.Interfaces;
using WhatsDown.Core.Interfaces.Networking;

namespace WhatsDown.Server.Networking.Clients.Tcp;

internal class TcpServerCommunicationHandler : INetworkCommunication
{
    // Services
    private readonly ILogger logger;

    private readonly TcpClient socket;
    private readonly EncryptionHandler encryption;
    private readonly TaskCompletionSource<bool> encryptionReady;
    private readonly CancellationTokenSource disconnectedCts;
    private NetworkStream Ns { get => socket.GetStream(); }

    public TcpServerCommunicationHandler(TcpClient socket)
    {
        logger = ServiceLocator.ServiceProvider.GetRequiredService<ILogger>();
        this.socket = socket;
        encryption = new EncryptionHandler();
        encryptionReady = new TaskCompletionSource<bool>();
        disconnectedCts = new CancellationTokenSource();

        InitializeEncryption();
    }

    private async void InitializeEncryption()
    {
        if (await EstablishEncryption())
            encryptionReady.SetResult(true);
    }

    public async Task WriteMessage(string message)
    {
        await encryptionReady.Task;

        byte[] writeBuffer = EncodingConfiguration.EncodingScheme.GetBytes(message);
        try
        {
            WriteBytes(writeBuffer);
        }
        catch
        {
            disconnectedCts.Cancel();
            throw new DisconnectedFromEndPointException();
        }
    }

    public async Task<string> ReadMessage()
    {
        await encryptionReady.Task;

        byte[] readBuffer;
        try
        {
            readBuffer = await ReadBytes();
        }
        catch
        {
            disconnectedCts.Cancel();
            throw new DisconnectedFromEndPointException();
        }
        return EncodingConfiguration.EncodingScheme.GetString(readBuffer);
    }

    private async Task<bool> EstablishEncryption()
    {
        try
        {
            // Import Rsa Details
            byte[] rsaPublicKey = await ReadBytes();
            encryption.ImportRsa(rsaPublicKey);

            // Send Aes Details
            byte[] aesPrivateKey = encryption.ExportAesPrivateKey();
            byte[] encryptedRsaPrivateKey = encryption.EncryptRsa(aesPrivateKey);
            WriteBytes(encryptedRsaPrivateKey);
            byte[] aesIv = encryption.ExportAesIv();
            byte[] encryptedRsaIv = encryption.EncryptRsa(aesIv);
            WriteBytes(encryptedRsaIv);

            // Test Encryption: Send
            string msgTest = "SUCCESS";
            byte[] decryptedTest = EncodingConfiguration.EncodingScheme.GetBytes(msgTest);
            byte[] encryptedTest = encryption.EncryptAes(decryptedTest);
            WriteBytes(encryptedTest);

            // Test Encryption: Receive
            encryptedTest = await ReadBytes();
            decryptedTest = encryption.DecryptAes(encryptedTest);
            msgTest = EncodingConfiguration.EncodingScheme.GetString(decryptedTest);

            if (msgTest != "SUCCESS")
                throw new EncryptionFailedException();
        }
        catch (Exception ex)
        {
            logger.LogError($"Failed Establishing Encryption: {ex.Message}");
            disconnectedCts.Cancel();
            return false;
        }
        logger.LogSuccess("Encryption Successful");
        return true;
    }

    private async void WriteBytes(byte[] bytes)
    {
        byte[] prefixedBuffer = PrefixLength(bytes);
        try
        {
            await Ns.WriteAsync(prefixedBuffer, disconnectedCts.Token);
        }
        catch { throw new DisconnectedFromEndPointException(); }
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
        catch { throw new DisconnectedFromEndPointException(); }
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
}