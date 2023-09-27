using System;
using System.IO;
using System.Threading.Tasks;
using WhatsDown.Core.Exceptions;
using WhatsDown.Core.Interfaces;
using WhatsDown.WPF.Interfaces.AppConfiguration;
using WhatsDown.WPF.Utils;

namespace WhatsDown.WPF.DI.Services.Configuration;

internal class ApplicationConfigurationFileManagerService : IApplicationConfigurationFileManager, IDisposable
{
	private const string FileName = "appconfig.json";

	private readonly ISerializer serializer;
	private readonly ILogger logger;

	private readonly FileSystemWatcher configFileWatcher = new FileSystemWatcher();
	private bool controlledOperation;

	private event Action? _configFileChanged;
	public event Action? ConfigFileChanged
	{
		add
		{
			_configFileChanged += value;
			_configFileChanged?.Invoke();
		}
		remove
		{
			_configFileChanged -= value;
			_configFileChanged?.Invoke();
		}
	}

	public ApplicationConfigurationFileManagerService(ISerializer serializer, ILogger logger)
	{
		this.serializer = serializer;
		this.logger = logger;

		configFileWatcher.Path = Directory.GetCurrentDirectory();
		configFileWatcher.Filter = FileName;

		configFileWatcher.Changed += (s, e) => HandleFileChanges();
		configFileWatcher.EnableRaisingEvents = true;
	}

	private async void HandleFileChanges()
	{
		if (controlledOperation)
		{
			controlledOperation = false;
			return;
		}

		string fullPath = Path.Combine(Directory.GetCurrentDirectory(), FileName);
		while (true)
		{
			try
			{
				new FileStream(fullPath, FileMode.Open, FileAccess.Read, FileShare.Read).Dispose();
				break;
			}
			catch
			{
				await Task.Delay(50);
			}

		}
		_configFileChanged?.Invoke();
	}

	public void SaveConfig(AppSettingsModel appSettings)
	{
		try
		{
			string serializedConfig = serializer.Serialize(appSettings)
				?? throw new SerializationException(appSettings);
			controlledOperation = true;
			File.WriteAllText(FileName, serializedConfig);
		}
		catch (SerializationException ex)
		{
			logger.LogError($"Error Saving Configuration File: {ex.Message}");
		}
	}

	public AppSettingsModel LoadConfig()
	{
		try
		{
			if (File.Exists(FileName))
			{
				controlledOperation = true;
				string serializedConfig = File.ReadAllText(FileName);
				return serializer.Deserialize<AppSettingsModel>(serializedConfig)
					?? throw new DeserializationException(serializedConfig);
			}
		}
		catch (DeserializationException ex)
		{
			logger.LogError($"Error Loading Configuration File: {ex.Message}");
		}
		SaveConfig(new AppSettingsModel());
		return LoadConfig();
	}

	public void Dispose()
	{
		configFileWatcher.Dispose();
	}
}