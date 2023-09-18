namespace WhatsDown.Server.Interfaces.Services.Factories;

internal interface IClientCommunicationFactory<TTransportType>
{
    IClientHandler CreateClient(TTransportType socket);
}