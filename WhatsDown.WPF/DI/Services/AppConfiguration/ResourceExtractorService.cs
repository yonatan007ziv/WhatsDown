using System.Windows;
using System.Windows.Media;
using WhatsDown.Core.Interfaces;
using WhatsDown.WPF.Interfaces.AppConfiguration;

namespace WhatsDown.WPF.DI.Services.Configuration;

internal class ResourceExtractorService : IResourceExtractor
{
	private readonly ILogger logger;

	public ResourceExtractorService(ILogger logger)
	{
		this.logger = logger;
	}

	public string GetString(string resourceKey)
	{
		try
		{
			return (string)Application.Current.Resources[resourceKey];
		}
		catch
		{
			logger.LogError($"Resource was Not Found, Key: {resourceKey}");
			return "";
		}
	}

	public Color GetColor(string resourceKey)
	{
		try
		{
			return (Color)Application.Current.Resources[resourceKey];
		}
		catch
		{
			logger.LogError($"Resource was Not Found, Key: {resourceKey}");
			return Colors.Black;
		}
	}
}