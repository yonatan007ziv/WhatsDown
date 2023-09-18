using WhatsDown.Server.Interfaces.Services;

namespace WhatsDown.Server.Mockers;

internal class MockConfigurationFetcher : IConfigurationFetcher
{
    public object GetAttribute(string route)
    {
        return new object();
    }

    public int GetIntAttribute(string route)
    {
        return 0;
    }

    public string GetStringAttribute(string route)
    {
        return "";
    }
}