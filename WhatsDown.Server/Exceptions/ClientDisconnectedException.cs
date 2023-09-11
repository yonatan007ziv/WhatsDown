namespace WhatsDown.Server.Exceptions;

internal class ClientDisconnectedException : Exception
{
    public ClientDisconnectedException()
        : base("Client Disconnected")
    {

    }
}