using System;
using WhatsDown.WPF.Utils;

namespace WhatsDown.WPF.Interfaces.AppConfiguration;

internal interface IApplicationConfigurationFileManager
{
	public event Action ConfigFileChanged;
	void SaveConfig(AppSettingsModel config);
	AppSettingsModel LoadConfig();
}