using System.Net.Sockets;
using WhatsDown.Core.CommunicationProtocol;
using WhatsDown.Core.Exceptions;
using WhatsDown.Core.Interfaces;
using WhatsDown.Core.Interfaces.Networking;
using WhatsDown.Core.Interfaces.Networking.Communication;
using WhatsDown.Core.Services.NetworkingEncoders.StringEncodingConfiguration;

namespace WhatsDown.Core.NetworkingShared;

public abstract class BaseTcpCommunication : INetworkCommunication
{
    private readonly IMessageEncoder<MessagePacket> encoder;
    private readonly ILogger logger;

    private NetworkStream Ns { get => client.GetStream(); }
    protected const string EncryptionTestWord = "Success";
    protected TcpClient client;
    protected IEncryptionHandler encryption;
    protected CancellationTokenSource disconnectedCts;
    protected TaskCompletionSource encryptionTask;

    public BaseTcpCommunication(TcpClient client, IMessageEncoder<MessagePacket> encoder, ILogger logger)
    {
        this.client = client;
        this.encoder = encoder;
        this.logger = logger;
        encryption = new EncryptionHandler();
        disconnectedCts = new CancellationTokenSource();
        encryptionTask = new TaskCompletionSource();
    }

    public async Task InitializeEncryption()
    {
        if (!await EstablishEncryption())
            throw new EncryptionFailedException();
        encryptionTask.SetResult();
    }

    protected abstract Task<bool> EstablishEncryption();

    public async Task WriteMessage(MessagePacket message)
    {
        await encryptionTask.Task;

        try
        {
            string encodedMessage = encoder.EncodeMessage(message)
                ?? throw new NetworkedMessageEncodingException(message);
            byte[] decryptedWriteBuffer = TextEncodingConfiguration.EncodingScheme.GetBytes(encodedMessage);
            byte[] writeBuffer = encryption.EncryptAes(decryptedWriteBuffer);
            await WriteBytes(writeBuffer);
        }
        catch (Exception ex)
        {
            logger.LogError($"Write Exception: {ex.Message}");
            throw new NetworkedWriteException();
        }
    }

    public async Task<MessagePacket?> ReadMessage()
    {
        await encryptionTask.Task;

        try
        {
            byte[] encryptedReadBuffer = await ReadBytes();
            byte[] readBuffer = encryption.DecryptAes(encryptedReadBuffer);
            string message = TextEncodingConfiguration.EncodingScheme.GetString(readBuffer);
            return encoder.DecodeMessage(message)
                ?? throw new NetworkedMessageDecodingException(message);
        }
        catch (Exception ex)
        {
            logger.LogError($"Read Exception: {ex.Message}");
            throw new NetworkedReadException();
        }
    }

    protected async Task WriteBytes(byte[] writeBuffer)
    {
        try
        {
            // Prefixes 4 Bytes Indicating Message Length
            byte[] length = BitConverter.GetBytes(writeBuffer.Length);
            byte[] prefixedBuffer = new byte[writeBuffer.Length + sizeof(int)];

            Array.Copy(length, 0, prefixedBuffer, 0, sizeof(int));
            Array.Copy(writeBuffer, 0, prefixedBuffer, sizeof(int), writeBuffer.Length);

            await Ns.WriteAsync(prefixedBuffer, disconnectedCts.Token);
        }
        catch
        {
            throw new NetworkedWriteException();
        }
    }

    protected async Task<byte[]> ReadBytes()
    {
        try
        {
            // Reads 4 Bytes Indicating Message Length
            byte[] lengthBuffer = new byte[4];
            await Ns.ReadAsync(lengthBuffer, disconnectedCts.Token);

            int length = BitConverter.ToInt32(lengthBuffer);
            byte[] readBufer = new byte[length];
            int bytesRead = await Ns.ReadAsync(readBufer, disconnectedCts.Token);

            if (bytesRead == 0)
                throw new NetworkedEndPointDisconnectedException();

            return readBufer;
        }
        catch
        {
            throw new NetworkedReadException();
        }
    }
}