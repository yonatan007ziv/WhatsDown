using WhatsDown.Core.CommunicationProtocol;

namespace WhatsDown.Core.Interfaces.Networking;

public interface INetworkCommunication
{
	Task WriteMessage(MessagePacket msg);
	Task<MessagePacket> ReadMessage();
}