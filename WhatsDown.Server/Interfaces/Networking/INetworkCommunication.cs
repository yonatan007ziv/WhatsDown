namespace WhatsDown.Server.Interfaces.Client;

internal interface INetworkCommunication
{
    Task Handle();
    void Terminate();
    void WriteMessage(string msg);
    Task<string> ReadMessage();
}