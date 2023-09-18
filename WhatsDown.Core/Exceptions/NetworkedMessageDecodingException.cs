namespace WhatsDown.Core.Exceptions;

internal class NetworkedMessageDecodingException : Exception
{
    public NetworkedMessageDecodingException(string msg)
        : base($"Could not Decode Message: {msg}")
    {

    }
}