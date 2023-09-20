using System.Windows.Media;

namespace WhatsDown.WPF.Interfaces.AppConfiguration;

internal interface IResourceExtractor
{
    string GetString(string resourceKey);
    Color GetColor(string resourceKey);
}