namespace WhatsDown.Server.Interfaces.Services;

internal interface IConfigurationService
{
    object GetAttribute(string route);
    int GetIntAttribute(string route);
    string GetStringAttribute(string route);
}