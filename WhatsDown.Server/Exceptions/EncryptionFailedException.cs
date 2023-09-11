namespace WhatsDown.Server.Exceptions;

internal class EncryptionFailedException : Exception
{
    public EncryptionFailedException()
        :base("Encryption Process Failed")
    {
        
    }
}