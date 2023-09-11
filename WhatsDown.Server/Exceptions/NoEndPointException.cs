namespace WhatsDown.Server.Exceptions;

internal class NoEndPointException : Exception
{
    public NoEndPointException() : base("Couldn't Find Client's EndPoint")
    {

    }
}