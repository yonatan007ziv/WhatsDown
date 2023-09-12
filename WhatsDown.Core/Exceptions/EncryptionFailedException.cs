namespace WhatsDown.Core.Exceptions;

public class EncryptionFailedException : Exception
{
    public EncryptionFailedException()
        :base("End-to-End Encryption Failed")
    {
        
    }
}