namespace WhatsDown.Core.Interfaces.Networking;

public interface INetworkCommunication
{
    Task WriteMessage(string msg);
    Task<string> ReadMessage();
}