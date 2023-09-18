using System.Net;
using System.Net.Sockets;
using WhatsDown.Core.Interfaces;
using WhatsDown.Server.Interfaces.Services;
using WhatsDown.Server.Interfaces.Services.Factories;

namespace WhatsDown.Server.Services.Networking;

internal class TcpLoopListenerService : ISocketListener
{
    private readonly IClientCommunicationFactory<TcpClient> clientFactory;

    // Services
    private readonly ILogger logger;

    private readonly List<ClientHandler> connectedClients;
    private readonly TcpListener listener;
    private readonly IPAddress addr;
    private readonly int port;

    public TcpLoopListenerService(IClientCommunicationFactory<TcpClient> clientFactory, IConfigurationFetcher configuration, ILogger logger)
    {
        this.clientFactory = clientFactory;
        this.logger = logger;

        connectedClients = new List<ClientHandler>();
        addr = IPAddress.Parse(configuration.GetStringAttribute("Server:Ip"));
        port = configuration.GetIntAttribute("Server:Port");

        listener = new TcpListener(addr, port);
    }

    public async Task StartListening()
    {
        listener.Start();
        logger.LogTrace($"{nameof(TcpLoopListenerService)} Started Listening on {addr}:{port}");

        while (true)
        {
            try
            {
                _ = clientFactory.CreateClient(await listener.AcceptTcpClientAsync()).ReadMessageLoop();
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                continue;
            }
        }
    }
}