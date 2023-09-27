namespace WhatsDown.Core.Interfaces.Networking;

public interface INetworkClientHandler : IBaseNetworkCommunication
{
	void Disconnect();
}