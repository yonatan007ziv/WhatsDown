using System.Security.Cryptography;
using WhatsDown.Core.Interfaces;

namespace WhatsDown.Core.Components;

public class EncryptionHandler : IEncryptionHandler
{
    public RSA Rsa { get; private set; } = RSA.Create();
    public Aes Aes { get; private set; } = Aes.Create();

    public byte[] EncryptAes(byte[] buffer)
    {
        return Aes.CreateEncryptor().TransformFinalBlock(buffer, 0, buffer.Length);
    }

    public byte[] EncryptRsa(byte[] buffer)
    {
        return Rsa.EncryptValue(buffer);
    }

    public byte[] DecryptAes(byte[] buffer)
    {
        return Aes.CreateDecryptor().TransformFinalBlock(buffer, 0, buffer.Length);
    }

    public byte[] DecryptRsa(byte[] buffer)
    {
        return Rsa.DecryptValue(buffer);
    }

    public byte[] ExportAes()
    {
        return Aes.Key.Concat(Aes.IV).ToArray();
    }

    public void ImportRsa(byte[] rsaPublicKey)
    {
        Rsa.ImportRSAPublicKey(rsaPublicKey, out _);
    }
}