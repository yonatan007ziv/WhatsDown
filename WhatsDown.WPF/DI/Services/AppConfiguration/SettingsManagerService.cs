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
	private AppSettingsModel configuration;

	public SettingsManagerService(IApplicationConfigurationFileManager configFileManager, ILogger logger)
	{
		this.configFileManager = configFileManager;
		this.logger = logger;

		configFileManager.ConfigFileChanged += RefreshSettings;
		configuration = configFileManager.LoadConfig();
	}

	public string GetCulture()
	{
		return configFileManager.LoadConfig().Localization;
	}

	public void SetCulture(string cultureName)
	{
		configuration.Localization = cultureName;
		configFileManager.SaveConfig(configuration);
	}

	public void RefreshSettings()
	{
		configuration = configFileManager.LoadConfig();
		SwitchCulture(configuration.Localization);
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