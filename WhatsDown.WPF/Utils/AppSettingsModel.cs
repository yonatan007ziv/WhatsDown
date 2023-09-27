namespace WhatsDown.WPF.Utils;

internal class AppSettingsModel
{
	public string Localization { get; set; }

	public AppSettingsModel()
	{
		Localization = "en-US";
	}

	public override string ToString()
	{
		return $"\nLocalization: {Localization}\n";
	}
}