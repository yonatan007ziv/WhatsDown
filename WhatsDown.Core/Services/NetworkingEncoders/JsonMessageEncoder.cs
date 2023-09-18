using System.Text.Json;
using WhatsDown.Core.Interfaces;
using WhatsDown.Core.Interfaces.Networking.Communication;

namespace WhatsDown.Core.Services.NetworkingEncoders;

public class JsonMessageEncoder<TMessage> : IMessageEncoder<TMessage> where TMessage : class
{
    private readonly ILogger logger;

    public JsonMessageEncoder(ILogger logger)
    {
        this.logger = logger;
    }

    public string? EncodeMessage(TMessage message)
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

    public TMessage? DecodeMessage(string message) 
    {
        try
        {
            return JsonSerializer.Deserialize<TMessage>(message)
                ?? throw new JsonException();
        }
        catch (JsonException)
        {
            logger.LogError($"JsonException While Deserializing: {message}");
            return null;
        }
    }
}