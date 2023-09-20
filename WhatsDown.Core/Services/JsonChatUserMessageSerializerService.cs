using System.Text.Json;
using WhatsDown.Core.Interfaces;
using WhatsDown.Core.Models;

namespace WhatsDown.Core.Services;

public class JsonChatUserMessageSerializerService : IChatUserMessageSerializer
{
	private readonly ILogger logger;

	public JsonChatUserMessageSerializerService(ILogger logger)
	{
		this.logger = logger;
	}

	public string? Serialize(IEnumerable<ChatModel> message)
	{
		return Serialize<IEnumerable<ChatModel>>(message);
	}

	public string? Serialize(ChatModel message)
	{
		return Serialize<ChatModel>(message);
	}

	public string? Serialize(UserModel message)
	{
		return Serialize<UserModel>(message);
	}

	public string? Serialize(MessageModel message)
	{
		return Serialize<MessageModel>(message);
	}

	public IEnumerable<ChatModel>? Deserialize(string message)
	{
		return Deserialize<IEnumerable<ChatModel>>(message);
	}

	ChatModel? ISerializer<ChatModel>.Deserialize(string message)
	{
		return Deserialize<ChatModel>(message);
	}

	UserModel? ISerializer<UserModel>.Deserialize(string message)
	{
		return Deserialize<UserModel>(message);
	}

	MessageModel? ISerializer<MessageModel>.Deserialize(string message)
	{
		return Deserialize<MessageModel>(message);
	}

	private string? Serialize<T>(T message)
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

	private T? Deserialize<T>(string message) where T : class
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