using WhatsDown.Server.Exceptions;

namespace WhatsDown.Server.Services;

internal class ConfigurationFetcherService : Interfaces.Services.IConfigurationFetcher
{
    private readonly Microsoft.Extensions.Configuration.IConfiguration config;

    public ConfigurationFetcherService(Microsoft.Extensions.Configuration.IConfiguration config)
    {
        this.config = config;
    }

    public object GetAttribute(string route)
    {
        return config[route] ?? throw new ConfigurationAttributeNotFound(route);
    }

    public int GetIntAttribute(string route)
    {
        int value = int.Parse(config[route] ?? throw new ConfigurationAttributeNotFound(route));
        return value;
    }

    public string GetStringAttribute(string route)
    {
        return config[route] ?? throw new ConfigurationAttributeNotFound(route);
    }
}