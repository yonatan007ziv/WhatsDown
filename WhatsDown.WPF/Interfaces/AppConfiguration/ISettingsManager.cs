using WhatsDown.WPF.Utils;

namespace WhatsDown.WPF.Interfaces.AppConfiguration;

internal interface ISettingsManager
{
    public AppSettingsModel Configuration { get; }

    string GetCulture();
    void SetCulture(string cultureName);
    void RefreshSettings();
}