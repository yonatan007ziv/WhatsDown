namespace WhatsDown.Server.Exceptions;

internal class ConfigurationFileNotFoundException : Exception
{
	public ConfigurationFileNotFoundException() : base("IConfiguration Not Found")
	{

	}
}