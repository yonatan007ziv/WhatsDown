using System.Net.Sockets;
using WhatsDown.Core.CommunicationProtocol;
using WhatsDown.Core.Exceptions;
using WhatsDown.Core.Interfaces;
using WhatsDown.Core.Interfaces.Networking;
using WhatsDown.Core.NetworkingShared;
using WhatsDown.Core.Shared;

namespace WhatsDown.Server.Networking;

internal class TcpCommunication : BaseTcpCommunication, INetworkClientHandler
{
	private readonly ILogger logger;

	public TcpCommunication(TcpClient client, ISerializer encoder, IConfigurationFetcher configFetcher, ILogger logger)
		: base(client, encoder, configFetcher, logger)
	{
		this.logger = logger;
		_ = InitializeEncryption();
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
		catch (NetworkedReadException ex) when (ex.Type == ReadExceptionType.Timedout) { Disconnect(); return new MessagePacket { TimedOut = true }; }
		catch (NetworkedReadException) { Disconnect(); return new MessagePacket { InvalidPacket = true }; }

		return result;
	}

	protected override async Task<bool> EstablishEncryption()
	{
		try
		{
			// Import Rsa Details
			byte[] rsaPublicKey = await ReadBytes();
			encryption.ImportRsa(rsaPublicKey);

			// Send Aes Details
			byte[] aesPrivateKey = encryption.ExportAesPrivateKey();
			byte[] encryptedRsaPrivateKey = encryption.EncryptRsa(aesPrivateKey);
			_ = WriteBytes(encryptedRsaPrivateKey);
			byte[] aesIv = encryption.ExportAesIv();
			byte[] encryptedRsaIv = encryption.EncryptRsa(aesIv);
			_ = WriteBytes(encryptedRsaIv);

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

	public void Disconnect()
	{
		client.Close();
		disconnectedCts.Cancel();
	}
}