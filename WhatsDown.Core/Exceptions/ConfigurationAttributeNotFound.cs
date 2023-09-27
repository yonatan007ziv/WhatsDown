namespace WhatsDown.Core.Exceptions;

public class ConfigurationAttributeNotFound : Exception
{
	public ConfigurationAttributeNotFound(string? route)
		: base($"Configuration Attribute {(route == "" ? "[.]" : route)} not found")
	{

	}
}