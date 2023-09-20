using System.Net.Sockets;
using System.Security.Cryptography;
using System.Text;
using WhatsDown.Core.CommunicationProtocol;
using WhatsDown.Core.Exceptions;
using WhatsDown.Core.Interfaces;
using WhatsDown.Core.Interfaces.Networking;
using WhatsDown.Core.Shared;

namespace WhatsDown.Core.NetworkingShared;

public abstract class BaseTcpCommunication : INetworkCommunication
{
	private readonly ISerializer<MessagePacket> encoder;
	private readonly ILogger logger;

	private NetworkStream Ns { get => client.GetStream(); }
	protected const string EncryptionTestWord = "Success";
	protected TcpClient client;
	protected IEncryptionHandler encryption;
	protected CancellationTokenSource disconnectedCts;
	protected CancellationTokenSource timeoutCts;
	protected TaskCompletionSource encryptionTask;

	public BaseTcpCommunication(TcpClient client, ISerializer<MessagePacket> encoder, ILogger logger)
	{
		this.client = client;
		this.encoder = encoder;
		this.logger = logger;
		encryption = new EncryptionHandler();
		disconnectedCts = new CancellationTokenSource();
		timeoutCts = new CancellationTokenSource();
		encryptionTask = new TaskCompletionSource();
	}

	protected abstract Task<bool> EstablishEncryption();

	public async Task InitializeEncryption()
	{
		if (!await EstablishEncryption())
			throw new EncryptionFailedException();
		encryptionTask.SetResult();
	}

	public async Task WriteMessage(MessagePacket message)
	{
		await encryptionTask.Task;

		try
		{
			string encodedMessage = encoder.Serialize(message)
				?? throw new SerializationException(message);
			byte[] decryptedWriteBuffer = TextEncoding.EncodingScheme.GetBytes(encodedMessage);
			byte[] writeBuffer = encryption.EncryptAes(decryptedWriteBuffer);
			await WriteBytes(writeBuffer);
			return;
		}
		catch (SerializationException ex) { logger.LogError($"Serialization Exception: {ex.Message}"); throw new NetworkedWriteException(WriteExceptionType.SerializationFailed); }
		catch (EncoderFallbackException ex) { logger.LogError($"Encoding Exception: {ex.Message}"); throw new NetworkedWriteException(WriteExceptionType.TextEncodingFailed); }
		catch (CryptographicException ex) { logger.LogError($"Cryptographic Exception: {ex.Message}"); throw new NetworkedWriteException(WriteExceptionType.EncryptionFailed); }
		catch (IOException ex) { logger.LogError($"IO Exception: {ex.Message}"); throw new NetworkedWriteException(WriteExceptionType.IO); }
		catch (OperationCanceledException ex) { logger.LogError($"Operation Cancelled Exception: {ex.Message}"); throw new NetworkedWriteException(WriteExceptionType.OperationCancelled); }
		catch (ObjectDisposedException ex) { logger.LogError($"Disposed Exception: {ex.Message}"); throw new NetworkedWriteException(WriteExceptionType.Disposed); }
		catch (Exception ex) { logger.LogFatal($"MISHANDLED EXCEPTION: {ex.Message}"); throw new Exception("MISHANDLED"); }
	}

	public async Task<MessagePacket> ReadMessage()
	{
		await encryptionTask.Task;

		try
		{
			byte[] encryptedReadBuffer = await ReadBytes();
			byte[] readBuffer = encryption.DecryptAes(encryptedReadBuffer);
			string message = TextEncoding.EncodingScheme.GetString(readBuffer);
			return encoder.Deserialize(message)
				?? throw new DeserializationException(message);
		}
		catch (NetworkedReadException ex) when (ex.Type == ReadExceptionType.Disconnected)
		{ logger.LogInformation($"Client Disconnected Gracefully."); throw new NetworkedReadException(ReadExceptionType.Disconnected); }
		catch (IOException ex) { logger.LogError($"IO Exception: {ex.Message}"); throw new NetworkedReadException(ReadExceptionType.IO); }
		catch (OperationCanceledException ex)
		{
			if (timeoutCts.Token.IsCancellationRequested)
			{
				logger.LogError($"Timeout Exception: {ex.Message}");
				throw new NetworkedReadException(ReadExceptionType.Timedout);
			}
			logger.LogError($"Operation Cancelled Exception: {ex.Message}");
			throw new NetworkedReadException(ReadExceptionType.OperationCancelled);
		}
		catch (NetworkedReadException ex) { logger.LogError($"Read Exception: {ex.Message}"); throw ex; }
		catch (CryptographicException ex) { logger.LogError($"Cryptographic Exception: {ex.Message}"); throw new NetworkedReadException(ReadExceptionType.EncryptionFailed); }
		catch (DecoderFallbackException ex) { logger.LogError($"Decoding Exception: {ex.Message}"); throw new NetworkedReadException(ReadExceptionType.TextDecodingFailed); }
		catch (DeserializationException ex) { logger.LogError($"Deserialization Exception: {ex.Message}"); throw new NetworkedReadException(ReadExceptionType.DeserializationFailed); }
		catch (ObjectDisposedException ex) { logger.LogError($"Disposed Exception: {ex.Message}"); throw new NetworkedReadException(ReadExceptionType.Disposed); }
		catch (Exception ex) { logger.LogFatal($"MISHANDLED EXCEPTION: {ex.Message}"); throw new Exception("MISHANDLED"); }
	}

	protected async Task WriteBytes(byte[] writeBuffer)
	{
		// Prefixes 4 Bytes Indicating Message Length
		byte[] length = BitConverter.GetBytes(writeBuffer.Length);
		byte[] prefixedBuffer = new byte[writeBuffer.Length + sizeof(int)];

		Array.Copy(length, 0, prefixedBuffer, 0, sizeof(int));
		Array.Copy(writeBuffer, 0, prefixedBuffer, sizeof(int), writeBuffer.Length);

		await Ns.WriteAsync(prefixedBuffer, disconnectedCts.Token);
	}

	protected async Task<byte[]> ReadBytes()
	{
		timeoutCts = new CancellationTokenSource();
		timeoutCts.CancelAfter(TimeSpan.FromSeconds(NetworkingConstants.ReadTimeoutSeconds));

		CancellationTokenSource joinedCts = CancellationTokenSource.CreateLinkedTokenSource(timeoutCts.Token, disconnectedCts.Token);

		// Reads 4 Bytes Indicating Message Length
		byte[] lengthBuffer = new byte[4];
		await Ns.ReadAsync(lengthBuffer, joinedCts.Token);

		int length = BitConverter.ToInt32(lengthBuffer);
		byte[] readBufer = new byte[length];
		int bytesRead = await Ns.ReadAsync(readBufer, joinedCts.Token);

		if (bytesRead == 0)
			throw new NetworkedReadException(ReadExceptionType.Disconnected);

		return readBufer;
	}
}