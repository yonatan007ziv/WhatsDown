using System;
using System.Linq;
using System.Windows;
using WhatsDown.Core.Interfaces;
using WhatsDown.WPF.Interfaces.AppConfiguration;
using WhatsDown.WPF.Utils;

namespace WhatsDown.WPF.DI.Services.Configuration;

internal class SettingsManagerService : ISettingsManager
{
	private readonly IApplicationConfigurationFileManager configFileManager;
	private readonly ILogger logger;

	public AppSettingsModel Configuration { get; set; }

	public SettingsManagerService(IApplicationConfigurationFileManager configFileManager, ILogger logger)
	{
		this.configFileManager = configFileManager;
		this.logger = logger;

		configFileManager.ConfigFileChanged += RefreshSettings;
		Configuration = configFileManager.LoadConfig();
	}

	public string GetCulture()
	{
		return configFileManager.LoadConfig().Localization;
	}

	public void SetCulture(string cultureName)
	{
		Configuration.Localization = cultureName;
		configFileManager.SaveConfig(Configuration);
	}

	public void RefreshSettings()
	{
		Configuration = configFileManager.LoadConfig();
		SwitchCulture(Configuration.Localization);
	}

	private void SwitchCulture(string cultureName)
	{
		try
		{
			ResourceDictionary newResourceDictionary = new ResourceDictionary
			{
				Source = new Uri($"/Resources/Languages/Strings.{cultureName}.xaml", UriKind.Relative)
			};

			ResourceDictionary? existingResourceDictionary =
				Application.Current.Resources.MergedDictionaries.FirstOrDefault(rd => rd.Source.OriginalString.Contains("Languages/Strings"));

			if (existingResourceDictionary != null)
				Application.Current.Resources.MergedDictionaries.Remove(existingResourceDictionary);

			Application.Current.Resources.MergedDictionaries.Add(newResourceDictionary);
		}
		catch (Exception ex)
		{
			logger.LogError($"Failed to Switch Language: {ex.Message}");
		}
	}
}