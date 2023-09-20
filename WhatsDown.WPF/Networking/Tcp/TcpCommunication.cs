using System;
using System.Net;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Threading;
using System.Threading.Tasks;
using WhatsDown.Core.CommunicationProtocol;
using WhatsDown.Core.CommunicationProtocol.Enums;
using WhatsDown.Core.Exceptions;
using WhatsDown.Core.Interfaces;
using WhatsDown.Core.NetworkingShared;
using WhatsDown.Core.Shared;
using WhatsDown.WPF.Interfaces;

namespace WhatsDown.WPF.Networking.Tcp;

class TcpCommunication : BaseTcpCommunication, INetworkClient
{
    private readonly ILogger logger;

    public string IntegrityToken { get; set; }

    public TcpCommunication(ISerializer<MessagePacket> encoder, ILogger logger)
        : base(new TcpClient(), encoder, logger)
    {
        this.logger = logger;
        IntegrityToken = "";
    }


	public async Task<bool> ValidateToken()
	{
        await WriteMessage(new MessagePacket(CommunicationType.TokenValidationRequest, IntegrityToken));
		MessagePacket received = await ReadMessage();
        return received.Type == CommunicationType.TokenValidationResponse
            && received.ExtractParamAsEnum<TokenResult>(0) == TokenResult.Success;
	}

	public async new Task WriteMessage(MessagePacket msg)
	{
        try
        {
            await base.WriteMessage(msg);
        }
        catch (NetworkedWriteException) { Disconnect(); }
	}

    public async new Task<MessagePacket> ReadMessage()
    {
        MessagePacket result;
        try
        {
            result = await base.ReadMessage();
        }
        catch (NetworkedReadException) { Disconnect(); return new MessagePacket { Result = CommunicationValid.No }; }

        return result;
    }

	protected override async Task<bool> EstablishEncryption()
    {
        try
        {
			// Send Rsa Details
			byte[] rsaPublicKey = encryption.ExportRsa();
            _ = WriteBytes(rsaPublicKey);

            // Import Aes Details
			byte[] encryptedAesPrivateKey = await ReadBytes();
            byte[] aesPrivateKey = encryption.DecryptRsa(encryptedAesPrivateKey);
            encryption.ImportAesPrivateKey(aesPrivateKey);
            byte[] encryptedAesIv = await ReadBytes();
            byte[] aesIv = encryption.DecryptRsa(encryptedAesIv);
            encryption.ImportAesIv(aesIv);

            // Test Encryption: Send
            string msgTest = EncryptionTestWord;
            byte[] decryptedTest = TextEncoding.EncodingScheme.GetBytes(msgTest);
            byte[] encryptedTest = encryption.EncryptAes(decryptedTest);
            _ = WriteBytes(encryptedTest);

            // Test Encryption: Receive
            encryptedTest = await ReadBytes();
            decryptedTest = encryption.DecryptAes(encryptedTest);
            msgTest = TextEncoding.EncodingScheme.GetString(decryptedTest);

            if (msgTest != EncryptionTestWord)
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

    /// <exception cref="SocketException"/>
    /// <exception cref="EncryptionFailedException"/>
    /// <exception cref="DisconnectedFromEndPointException"/>
    public async Task<bool> Connect(IPAddress addr, int port)
    {
        try
        {
            await client.ConnectAsync(addr, port);
            await InitializeEncryption();
            return true;
        }
        catch (SocketException ex) { logger.LogError($"Connection Failed: {ex.Message}"); }
        catch (EncryptionFailedException ex) { logger.LogError($"Encryption Failed: {ex.Message}"); }
        catch (NetworkedWriteException ex) { logger.LogError($"Disconnected (While Write): {ex.Message}"); }
        catch (NetworkedReadException ex) { logger.LogError($"Disconnected (While Read): {ex.Message}"); }
        catch (CryptographicException ex) { logger.LogError($"Disconnected (While initializing Encryption): {ex.Message}"); }
        return false;
    }

    public void Disconnect()
    {
        client.Close();
		disconnectedCts.Cancel();
		timeoutCts.Cancel();
		client = new TcpClient();
        encryption = new EncryptionHandler();
        disconnectedCts = new CancellationTokenSource();
        encryptionTask = new TaskCompletionSource();
    }

    public void Dispose()
    {
        Disconnect();
    }
}