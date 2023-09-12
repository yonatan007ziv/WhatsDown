namespace WhatsDown.Core.Exceptions;

public class DisconnectedFromEndPointException : Exception
{
    public DisconnectedFromEndPointException()
        : base("Unhandled Disconnection From EndPoint")
    {

    }
}