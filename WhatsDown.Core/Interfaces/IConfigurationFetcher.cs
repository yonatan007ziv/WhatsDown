namespace WhatsDown.Core.Interfaces;

public interface IConfigurationFetcher
{
	object GetAttribute(string route);
	int GetIntAttribute(string route);
	string GetStringAttribute(string route);
}