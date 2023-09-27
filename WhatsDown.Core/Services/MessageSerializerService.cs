using WhatsDown.Core.CommunicationProtocol;
using WhatsDown.Core.Interfaces;

namespace WhatsDown.Core.Services;

public class MessageSerializerService : IMessageSerializer
{
	private readonly ISerializer serializer;

	public MessageSerializerService(ISerializer serializer)
	{
		this.serializer = serializer;
	}

	public string? Serialize(MessagePacket message)
	{
		return serializer.Serialize(message);
	}

	public MessagePacket? Deserialize(string message)
	{
		return serializer.Deserialize<MessagePacket>(message);
	}
}