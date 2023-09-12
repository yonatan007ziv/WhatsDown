using Microsoft.Extensions.DependencyInjection;
using System.Net;
using System.Net.Sockets;
using WhatsDown.Core.Interfaces;
using WhatsDown.Server.Handlers;
using WhatsDown.Server.Interfaces.Services;

namespace WhatsDown.Server.Networking.Clients.Servers;

internal class TcpServer
{
    // Services
    private readonly ILogger logger;

    private readonly List<ClientCommunicationHandler> connectedClients;
    private readonly TcpListener listener;
    private readonly IPAddress addr;
    private readonly int port;

    public TcpServer()
    {
        IConfigurationService config = ServiceLocator.ServiceProvider.GetRequiredService<IConfigurationService>();
        logger = ServiceLocator.ServiceProvider.GetRequiredService<ILogger>();

        connectedClients = new List<ClientCommunicationHandler>();
        addr = IPAddress.Parse(config.GetStringAttribute("Server:Ip"));
        port = config.GetIntAttribute("Server:Port");

        listener = new TcpListener(addr, port);
    }

    public async Task Start()
    {
        listener.Start();
        logger.LogTrace($"{nameof(TcpServer)} Started Listening on {addr}:{port}");

        while (true)
        {
            try
            {
                ClientCommunicationHandler client = new ClientCommunicationHandler(await listener.AcceptTcpClientAsync());
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                continue;
            }
        }
    }
}