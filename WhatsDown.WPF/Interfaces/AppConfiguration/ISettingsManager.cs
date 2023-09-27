namespace WhatsDown.WPF.Interfaces.AppConfiguration;

internal interface ISettingsManager
{
	string GetCulture();
	void SetCulture(string cultureName);
	void RefreshSettings();
}