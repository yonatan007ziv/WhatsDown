namespace WhatsDown.Core.Interfaces;

public interface IEncryptionHandler
{
    void ImportRsa(byte[] rsaPublicKey);
    byte[] ExportAes();

    byte[] EncryptRsa(byte[] buffer);
    byte[] DecryptRsa(byte[] buffer);
    byte[] EncryptAes(byte[] buffer);
    byte[] DecryptAes(byte[] buffer);
}