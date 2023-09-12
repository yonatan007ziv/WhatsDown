namespace WhatsDown.Core.Interfaces;

public interface IEncryptionHandler
{
    byte[] EncryptRsa(byte[] buffer);
    byte[] DecryptRsa(byte[] buffer);
    byte[] EncryptAes(byte[] buffer);
    byte[] DecryptAes(byte[] buffer);

    void ImportRsa(byte[] rsaPublicKey);
    byte[] ExportRsa();
    void ImportAesPrivateKey(byte[] rsaPublicKey);
    void ImportAesIv(byte[] rsaPublicKey);
    byte[] ExportAesPrivateKey();
    byte[] ExportAesIv();
}