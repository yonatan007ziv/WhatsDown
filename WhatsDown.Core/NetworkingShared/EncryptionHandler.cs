using System.Security.Cryptography;
using WhatsDown.Core.Interfaces;

namespace WhatsDown.Core.NetworkingShared;

public class EncryptionHandler : IEncryptionHandler
{
    private readonly RSA rsa = RSA.Create();
    private readonly Aes aes = Aes.Create();

    public EncryptionHandler()
    {
        aes.Padding = PaddingMode.PKCS7;
        aes.Mode = CipherMode.CBC;
    }

    public byte[] EncryptAes(byte[] buffer)
    {
        return aes.CreateEncryptor().TransformFinalBlock(buffer, 0, buffer.Length);
    }

    public byte[] EncryptRsa(byte[] buffer)
    {
        return rsa.Encrypt(buffer, RSAEncryptionPadding.OaepSHA256);
    }

    public byte[] DecryptAes(byte[] buffer)
    {
        return aes.CreateDecryptor().TransformFinalBlock(buffer, 0, buffer.Length);
    }

    public byte[] DecryptRsa(byte[] buffer)
    {
        return rsa.Decrypt(buffer, RSAEncryptionPadding.OaepSHA256);
    }

    public void ImportRsa(byte[] rsaPublicKey)
    {
        rsa.ImportRSAPublicKey(rsaPublicKey, out _);
    }

    public byte[] ExportRsa()
    {
        return rsa.ExportRSAPublicKey();
    }

    public void ImportAesPrivateKey(byte[] aesPrivateKey)
    {
        aes.Key = aesPrivateKey;
    }

    public void ImportAesIv(byte[] aesIv)
    {
        aes.IV = aesIv;
    }

    public byte[] ExportAesPrivateKey()
    {
        return aes.Key;
    }

    public byte[] ExportAesIv()
    {
        return aes.IV;
    }
}