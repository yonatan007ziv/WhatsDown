using Microsoft.Extensions.DependencyInjection;
using System.Net.Sockets;
using WhatsDown.Server.Interfaces.Client;
using WhatsDown.Server.Interfaces.Services;
using WhatsDown.Server.Networking.Clients.Tcp;

namespace WhatsDown.Server.Handlers;

internal class ClientCommunicationHandler
{
    private readonly ILogger logger;

    private readonly INetworkCommunication client;

    public ClientCommunicationHandler(TcpClient socket)
    {
        logger = ServiceLocator.ServiceProvider.GetRequiredService<ILogger>();
        client = new TcpClientHandler(socket);
    }

    public async void HandleClient()
    {
        try
        {
            await client.Handle();
        }
        catch (Exception ex)
        {
            client.Terminate();
            logger.LogError($"Client Exception: {ex.Message}");
        }
    }

    public async void Read()
    {
        string msg = await client.ReadMessage();
    }
    public void Write(string msg)
    {
        client.WriteMessage(msg);
    }
}