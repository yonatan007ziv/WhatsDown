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

internal class LoginHandlerService : ILoginRequestResponseHandler
{
    private readonly INetworkClient client;
    private readonly ILogger logger;

    public LoginHandlerService(INetworkClient client, ILogger logger)
    {
        this.client = client;
        this.logger = logger;
    }

    public async Task<LoginResult> LoginProcedure(LoginModel model)
    {
        try
        {
            if (!await client.Connect(IPAddress.Parse("127.0.0.1"), 7777))
                return LoginResult.ServerUnreachable;
            await PostRequest(model);
            LoginResult result = await GetResponse();
            client.Disconnect();
            return result;
        }
        catch (Exception ex)
        {
            logger.LogInformation($"Error Occured in Login: {ex.Message}");
            return LoginResult.UnknownError;
        }
    }

    public async Task PostRequest(LoginModel model)
    {
        logger.LogInformation("Posting Login Request...");
        MessagePacket loginPacket = new MessagePacket(CommunicationPurpose.LoginRequest, model.Email, model.Password);
        await client.WriteMessage(loginPacket);
    }

    public async Task<LoginResult> GetResponse()
    {
        logger.LogInformation("Getting Login Response...");
        MessagePacket response = await client.ReadMessage();
        if (response.Type == CommunicationPurpose.LoginResponse)
            return response.ExtractParam<LoginResult>(0);
        return LoginResult.UnknownError;
    }

    public void Dispose()
    {
        client.Dispose();
    }
}