using Microsoft.Extensions.Configuration;
using WhatsDown.Core.Exceptions;
using WhatsDown.Core.Interfaces;

namespace WhatsDown.Core.Services;

public class ConfigurationFetcherService : IConfigurationFetcher
{
	private readonly IConfiguration config;

	public ConfigurationFetcherService(IConfiguration config)
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