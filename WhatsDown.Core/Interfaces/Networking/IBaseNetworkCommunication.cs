using WhatsDown.Core.CommunicationProtocol;

namespace WhatsDown.Core.Interfaces.Networking;

public interface IBaseNetworkCommunication
{
	Task WriteMessage(MessagePacket msg);
	Task<MessagePacket> ReadMessage();
}