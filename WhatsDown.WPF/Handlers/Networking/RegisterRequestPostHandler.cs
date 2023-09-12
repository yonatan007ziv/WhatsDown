using System;
using System.Net;
using System.Threading.Tasks;
using WhatsDown.Core.CommunicationProtocol.Enums;
using WhatsDown.Core.Interfaces;
using WhatsDown.WPF.Interfaces;
using WhatsDown.WPF.MVVM.Models;
using WhatsDown.WPF.Networking.Tcp;

namespace WhatsDown.WPF.Handlers.Networking;

class RegisterRequestPostHandler : IDisposable
{
    private readonly IResultCommunicator<RegisterResult> resultCommunicator;
    private readonly RegisterModel registerModel;
    private readonly INetworkClient client;

    public RegisterRequestPostHandler(ILogger logger, IResultCommunicator<RegisterResult> resultCommunicator, RegisterModel registerModel)
    {
        client = new TcpClientCommunicationHandler(logger);
        this.resultCommunicator = resultCommunicator;
        this.registerModel = registerModel;
    }

    public async Task InitiateRegisterRequest()
    {
        if (!await client.Connect(IPAddress.Parse("127.0.0.1"), 7777))
        {
            resultCommunicator.SetResult(RegisterResult.ServerUnreachable);
            return;
        }

        await client.WriteMessage($"REGISTER:{registerModel.Email} {registerModel.Password}");

        string result = await client.ReadMessage();
        resultCommunicator.SetResult(RegisterResult.Success);
    }

    public void Dispose()
    {
        client.Dispose();
    }
}