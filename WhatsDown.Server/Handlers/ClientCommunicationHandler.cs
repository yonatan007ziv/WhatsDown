using Microsoft.Extensions.DependencyInjection;
using System.Net.Sockets;
using WhatsDown.Core.Exceptions;
using WhatsDown.Core.Interfaces;
using WhatsDown.Core.Interfaces.Networking;
using WhatsDown.Server.Networking.Clients.Tcp;

namespace WhatsDown.Server.Handlers;

internal class ClientCommunicationHandler : INetworkCommunication
{
    private readonly ILogger logger;

    private readonly INetworkCommunication client;

    public ClientCommunicationHandler(TcpClient socket)
    {
        logger = ServiceLocator.ServiceProvider.GetRequiredService<ILogger>();
        client = new TcpServerCommunicationHandler(socket);

        ReadMessageLoop();
    }

    public async Task WriteMessage(string msg)
    {
        await client.WriteMessage(msg);
    }

    public async Task<string> ReadMessage()
    {
        return await client.ReadMessage();
    }

    private async void ReadMessageLoop()
    {
        while (true)
        {
            try
            {
                string msg = await ReadMessage();
                InterpretMessage(msg);
            }
            catch (DisconnectedFromEndPointException)
            {
                logger.LogError("Client Disconnected");
                break;
            }
        }
    }

    private void InterpretMessage(string msg)
    {
        logger.LogSuccess($"GOT MESSAGE! {msg}");
        WriteMessage("NIGGA");
    }
}