using System.Net.Sockets;
using WhatsDown.Core.CommunicationProtocol;
using WhatsDown.Core.Exceptions;
using WhatsDown.Core.Interfaces;
using WhatsDown.Core.Interfaces.Networking;
using WhatsDown.Core.Interfaces.Networking.Communication;
using WhatsDown.Core.NetworkingShared;
using WhatsDown.Core.Services.NetworkingEncoders.StringEncodingConfiguration;

namespace WhatsDown.Server.Networking;

internal class TcpCommunication : BaseTcpCommunication, INetworkCommunication
{
    private readonly ILogger logger;

    public TcpCommunication(TcpClient client, IMessageEncoder<MessagePacket> encoder, ILogger logger)
        : base(client, encoder, logger)
    {
        this.logger = logger;
        _ = InitializeEncryption();
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
            byte[] decryptedTest = TextEncodingConfiguration.EncodingScheme.GetBytes(msgTest);
            byte[] encryptedTest = encryption.EncryptAes(decryptedTest);
            _ = WriteBytes(encryptedTest);

            // Test Encryption: Receive
            encryptedTest = await ReadBytes();
            decryptedTest = encryption.DecryptAes(encryptedTest);
            msgTest = TextEncodingConfiguration.EncodingScheme.GetString(decryptedTest);

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
}