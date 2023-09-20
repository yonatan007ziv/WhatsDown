using System.Text.Json;
using WhatsDown.Core.Interfaces;
using WhatsDown.WPF.Utils;

namespace WhatsDown.WPF.DI.Services.Configuration;

internal class JsonAppConfigurationSerializerService : ISerializer<AppSettingsModel>
{
    private readonly ILogger logger;

    public JsonAppConfigurationSerializerService(ILogger logger)
    {
        this.logger = logger;
    }

    public string? Serialize(AppSettingsModel deserialized)
    {
        try
        {
            return JsonSerializer.Serialize(deserialized)
                ?? throw new JsonException();
        }
        catch (JsonException)
        {
            logger.LogError($"Error Serializing Configuration, Deserialized: {deserialized}");
            return default;
        }
    }

    public AppSettingsModel? Deserialize(string serialized)
    {
        try
        {
            return JsonSerializer.Deserialize<AppSettingsModel>(serialized)
                ?? throw new JsonException();
        }
        catch (JsonException)
        {
            logger.LogError($"Error Deserializing Configuration, Serialized: {serialized}");
            return null;
        }
    }
}