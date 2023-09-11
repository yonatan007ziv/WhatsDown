using Microsoft.Extensions.DependencyInjection;
using System.Net.Sockets;
using WhatsDown.Core.Components;
using WhatsDown.Core.Configuration;
using WhatsDown.Server.Exceptions;
using WhatsDown.Server.Interfaces.Client;
using WhatsDown.Server.Interfaces.Services;

namespace WhatsDown.Server.Networking.Clients.Tcp;

internal class TcpClientHandler : INetworkCommunication
{
    // Services
    private readonly ILogger logger;

    private readonly TcpClient socket;
    private readonly EncryptionHandler encryption;
    private readonly TaskCompletionSource<bool> encryptionReady;
    private readonly CancellationTokenSource disconnectedCts;
    private NetworkStream Ns { get => socket.GetStream(); }

    public TcpClientHandler(TcpClient socket)
    {
        logger = ServiceLocator.ServiceProvider.GetRequiredService<ILogger>();
        this.socket = socket;
        encryption = new EncryptionHandler();
        encryptionReady = new TaskCompletionSource<bool>();
        disconnectedCts = new CancellationTokenSource();
    }

    public async Task Handle()
    {
        encryptionReady.SetResult(await EstablishEncryption());
        //await ReadLoop();
    }

    public void Terminate()
    {
        try
        {
            socket.Client.Shutdown(SocketShutdown.Send);
            socket.Close();
            socket.Dispose();
        }
        catch (Exception ex)
        {
            logger.LogError($"Error While Terminating Connection: {ex.Message}");
        }
    }

    private async Task ReadLoop()
    {
        while (!disconnectedCts.IsCancellationRequested)
        {
            string message = await ReadMessage();
            logger.LogInformation($"Got Message: {message}");
        }
        logger.LogError($"Read Loop Stopped: CTS Raised");
    }

    public async void WriteMessage(string message)
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
            throw new ClientDisconnectedException();
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
            throw new ClientDisconnectedException();
        }
        return EncodingConfiguration.EncodingScheme.GetString(readBuffer);
    }

    private async Task<bool> EstablishEncryption()
    {
        try
        {
            // Import Rsa Details
            logger.LogTrace("Reading RSA Key...");
            byte[] rsaPublicKey = await ReadBytes();
            encryption.ImportRsa(rsaPublicKey);
            logger.LogInformation("Imported RSA Key!");

            // Send Aes Details
            logger.LogTrace("Exporting AES Key & IV...");
            byte[] aesPrivateKeyIv = encryption.ExportAes();
            WriteBytes(aesPrivateKeyIv);
            logger.LogInformation("Wrote AES Key & IV!");

            // Test Encryption
            logger.LogTrace("Testing End to End Encryption...");
            byte[] encryptedTest = await ReadBytes();
            byte[] decryptedTest = encryption.DecryptAes(encryptedTest);
            string msgTest = EncodingConfiguration.EncodingScheme.GetString(decryptedTest);
            logger.LogInformation($"Tested End to End Encryption! Result: {msgTest}");

            if (msgTest != "SUCCESS")
            {
                logger.LogError("Encryption Failed!");
                throw new EncryptionFailedException();
            }
        }
        catch (Exception ex)
        {
            logger.LogError($"Failed Establishing Encryption: {ex.Message}");
            disconnectedCts.Cancel();
        }
        return true;
    }

    private async void WriteBytes(byte[] bytes)
    {
        try
        {
            await Ns.WriteAsync(bytes, disconnectedCts.Token);
        }
        catch
        {
            throw new ClientDisconnectedException();
        }
    }

    private async Task<byte[]> ReadBytes()
    {
        byte[] buffer = new byte[socket.ReceiveBufferSize];
        int bytesRead;
        try
        {
            bytesRead = await Ns.ReadAsync(buffer, disconnectedCts.Token);
        }
        catch
        {
            throw new ClientDisconnectedException();
        }

        if (bytesRead == 0)
            throw new ClientDisconnectedException();

        return buffer;
    }
}