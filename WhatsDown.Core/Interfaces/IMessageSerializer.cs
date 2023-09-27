using WhatsDown.Core.CommunicationProtocol;

namespace WhatsDown.Core.Interfaces;

public interface IMessageSerializer
{
	string? Serialize(MessagePacket message);
	MessagePacket? Deserialize(string message);
}