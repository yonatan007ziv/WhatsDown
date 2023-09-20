using System.Text.Json;
using WhatsDown.Core.CommunicationProtocol;
using WhatsDown.Core.Interfaces;

namespace WhatsDown.Core.Services;

public class JsonMessageSerializerService : ISerializer<MessagePacket>
{
    private readonly ILogger logger;

    public JsonMessageSerializerService(ILogger logger)
    {
        this.logger = logger;
    }

    public string? Serialize(MessagePacket message)
    {
        try
        {
            return JsonSerializer.Serialize(message);
        }
        catch (JsonException)
        {
            logger.LogError($"JsonException While Serializing: {message}");
            return null;
        }
    }

    public MessagePacket? Deserialize(string message)
    {
        try
        {
            return JsonSerializer.Deserialize<MessagePacket>(message)
                ?? throw new JsonException();
        }
        catch (JsonException)
        {
            logger.LogError($"JsonException While Deserializing: {message}");
            return null;
        }
    }
}