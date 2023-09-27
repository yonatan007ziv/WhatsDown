using System.Text.Json;
using WhatsDown.Core.Interfaces;

namespace WhatsDown.Core.Services;

public class JsonSerializerService : ISerializer
{
	private readonly ILogger logger;

	public JsonSerializerService(ILogger logger)
	{
		this.logger = logger;
	}

	public string? Serialize<T>(T message) where T : class
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

	public T? Deserialize<T>(string message) where T : class
	{
		try
		{
			return JsonSerializer.Deserialize<T>(message)
				?? throw new JsonException();
		}
		catch (JsonException)
		{
			logger.LogError($"JsonException While Deserializing: {message}");
			return null;
		}
	}
}