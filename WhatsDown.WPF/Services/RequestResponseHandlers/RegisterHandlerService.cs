using System;
using System.Net;
using System.Threading.Tasks;
using WhatsDown.Core.CommunicationProtocol;
using WhatsDown.Core.CommunicationProtocol.Enums;
using WhatsDown.Core.Interfaces;
using WhatsDown.WPF.Interfaces;
using WhatsDown.WPF.Interfaces.RequestResponse;
using WhatsDown.WPF.MVVM.Models;

namespace WhatsDown.WPF.Services.RequestResponseHandlers;

internal class RegisterHandlerService : IRegisterRequestResponseHandler
{
    private readonly INetworkClient client;
    private readonly ILogger logger;

    public RegisterHandlerService(INetworkClient client, ILogger logger)
    {
        this.client = client;
        this.logger = logger;
    }

    public async Task<RegisterResult> RegisterProcedure(RegisterModel model)
    {
        try
        {
            if (!await client.Connect(IPAddress.Parse("127.0.0.1"), 7777))
                return RegisterResult.ServerUnreachable;
            await PostRequest(model);
            RegisterResult result = await GetResponse();
            client.Disconnect();
            return result;
        }
        catch (Exception ex)
        {
            logger.LogInformation($"Error Occured in Login: {ex.Message}");
            return RegisterResult.UnknownError;
        }
    }

    public async Task PostRequest(RegisterModel model)
    {
        logger.LogInformation("Posting Register Request...");
        MessagePacket loginPacket = new MessagePacket(CommunicationPurpose.LoginRequest, model.Email, model.Password);
        await client.WriteMessage(loginPacket);
    }

    public async Task<RegisterResult> GetResponse()
    {
        logger.LogInformation("Getting Register Response...");
        MessagePacket response = await client.ReadMessage();
        if (response.Type == CommunicationPurpose.RegisterResponse)
            return response.ExtractParam<RegisterResult>(0);
        return RegisterResult.UnknownError;
    }

    public void Dispose()
    {
        client.Dispose();
    }
}