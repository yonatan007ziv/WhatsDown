using System.Net;

namespace WhatsDown.Core.Interfaces.Networking;

public interface INetworkClient
{
	Task<bool> Connect(IPAddress addr, int port);
	void Disconnect();
}