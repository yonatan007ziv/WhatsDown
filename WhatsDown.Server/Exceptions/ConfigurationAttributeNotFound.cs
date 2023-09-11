namespace WhatsDown.Server.Exceptions;

internal class ConfigurationAttributeNotFound : Exception
{
    public ConfigurationAttributeNotFound(string? route)
        : base($"Configuration Attribute {(route == "" ? "[.]" : route)} not found")
    {

    }
}