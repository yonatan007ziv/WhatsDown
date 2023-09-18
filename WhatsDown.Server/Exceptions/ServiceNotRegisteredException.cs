namespace WhatsDown.Server.Exceptions;

internal class ServiceNotRegisteredException : Exception
{
    public ServiceNotRegisteredException(string serviceName)
        : base($"{serviceName} Service Not Registered")
    {

    }
}