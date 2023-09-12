using System.Net;
using System.Threading.Tasks;
using WhatsDown.Core.Interfaces;
using WhatsDown.Core.Interfaces.Networking;
using WhatsDown.WPF.Interfaces;
using WhatsDown.WPF.Networking.Tcp;

namespace WhatsDown.WPF.Handlers;
class ServerCommunicationHandler : INetworkCommunication
{
    private readonly INetworkClient client;

    public ServerCommunicationHandler(ILogger logger)
    {
        client = new TcpClientCommunicationHandler(logger);
    }

    private void InterpretMessage(string msg)
    {

    }

    public async Task<bool> Connect(IPAddress addr, int port)
    {
        try
        {
            await client.Connect(addr, port);

            return true;
        }
        catch { return false; }
    }

    public async Task WriteMessage(string msg)
    {
        await client.WriteMessage(msg);
    }

    public async Task<string> ReadMessage()
    {
        return await client.ReadMessage();
    }

    public async void ReadMessageLoop()
    {
        while (true)
        {
            string msg = await ReadMessage();
            InterpretMessage(msg);
        }
    }
}