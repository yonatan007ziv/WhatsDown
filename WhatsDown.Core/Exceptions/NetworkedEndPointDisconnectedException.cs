namespace WhatsDown.Core.Exceptions;

internal class NetworkedEndPointDisconnectedException : Exception
{
    public NetworkedEndPointDisconnectedException()
        : base("EndPoint Disconnected Gracefully")
    {

    }
}