using System;
using System.Net;
using System.Threading.Tasks;
using WhatsDown.Core.CommunicationProtocol.Enums;
using WhatsDown.Core.Interfaces;
using WhatsDown.WPF.Interfaces;
using WhatsDown.WPF.MVVM.Models;
using WhatsDown.WPF.Networking.Tcp;

namespace WhatsDown.WPF.Handlers.Networking;

class LoginRequestPostHandler : IDisposable
{
    private readonly IResultCommunicator<LoginResult> resultCommunicator;
    private readonly LoginModel loginModel;
    private readonly INetworkClient client;

    public LoginRequestPostHandler(ILogger logger, IResultCommunicator<LoginResult> resultCommunicator, LoginModel loginModel)
    {
        client = new TcpClientCommunicationHandler(logger);
        this.resultCommunicator = resultCommunicator;
        this.loginModel = loginModel;
    }

    public async Task InitiateLoginRequest()
    {
        if (!await client.Connect(IPAddress.Parse("127.0.0.1"), 7777))
        {
            resultCommunicator.SetResult(LoginResult.ServerUnreachable);
            return;
        }
        
        await client.WriteMessage($"LOGIN:{loginModel.Email} {loginModel.Password}");

        string result = await client.ReadMessage();
        resultCommunicator.SetResult(LoginResult.Success);
    }

    public void Dispose()
    {
        client.Dispose();
    }
}