using WhatsDown.Core.Interfaces;
using WhatsDown.Core.Interfaces.Networking;
using WhatsDown.Server.Interfaces.Services;
using WhatsDown.Server.Interfaces.Services.Factories;
using WhatsDown.Server.Services.Networking;

namespace WhatsDown.Server.Factories;

internal class ClientCommunicationFactory<TTransportType> : IClientCommunicationFactory<TTransportType>
{
    private readonly Func<TTransportType, INetworkCommunication> factory;
    private readonly ILogger logger;

    public ClientCommunicationFactory(Func<TTransportType, INetworkCommunication> factory, ILogger logger)
    {
        this.factory = factory;
        this.logger = logger;
    }

    public IClientHandler CreateClient(TTransportType socket)
    {
        return new ClientHandler(factory(socket), logger);
    }
}