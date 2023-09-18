using WhatsDown.Core.CommunicationProtocol;
using WhatsDown.Core.CommunicationProtocol.Enums;
using WhatsDown.Core.Interfaces;
using WhatsDown.Core.Interfaces.Networking;
using WhatsDown.Server.Interfaces.Services;

namespace WhatsDown.Server.Services.Networking;

internal class ClientHandler : IClientHandler
{
    private readonly ILogger logger;
    private readonly INetworkCommunication client;

    public ClientHandler(INetworkCommunication client, ILogger logger)
    {
        this.client = client;
        this.logger = logger;
    }

    public async Task ReadMessageLoop()
    {
        while (true)
        {
            MessagePacket received = await client.ReadMessage()
                ?? throw new Exception("temp");

            switch (received.Type)
            {
                case CommunicationPurpose.LoginRequest:
                    HandleLoginRequest(received);
                    break;
                case CommunicationPurpose.RegisterRequest:
                    HandleRegisterRequest(received);
                    break;
                default:
                    break;
            }
        }
    }

    private void HandleLoginRequest(MessagePacket context)
    {
        logger.LogInformation($"Got Login Request\nContext Params: {context.Param[0]},{context.Param[1]}");

        _ = client.WriteMessage(new MessagePacket(CommunicationPurpose.LoginResponse, LoginResult.Success.ToString()));
    }

    private void HandleRegisterRequest(MessagePacket context)
    {
        logger.LogInformation($"Got Register Request\nContext Params: {context.Param[0]},{context.Param[1]}");
    }
}