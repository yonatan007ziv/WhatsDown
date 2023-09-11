namespace WhatsDown.Server.Exceptions;

internal class ServiceInitializationException : Exception
{
    public ServiceInitializationException(string serviceName)
        : base($"{serviceName} Failed To Initialize")
    {

    }
}