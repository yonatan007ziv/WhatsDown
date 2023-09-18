using System.Threading.Tasks;
using WhatsDown.Core.CommunicationProtocol.Enums;
using WhatsDown.Core.Interfaces;
using WhatsDown.WPF.Interfaces.RequestResponse;
using WhatsDown.WPF.MVVM.Models;

namespace WhatsDown.WPF.Mockers;

internal class MockRegisterHandlerService : IRegisterRequestResponseHandler
{
    private readonly ILogger logger;

    private string Email { get; set; } = "";
    private string Password { get; set; } = "";

    public MockRegisterHandlerService(ILogger logger)
    {
        this.logger = logger;
    }

    public async Task<RegisterResult> RegisterProcedure(RegisterModel model)
    {
        logger.LogInformation($"REGISTER MOCK: email({Email}) password({Password})");
        await PostRequest(model);
        return await GetResponse();
    }

    public Task PostRequest(RegisterModel model)
    {
        Email = model.Email;
        Password = model.Password;
        return Task.CompletedTask;
    }

    public async Task<RegisterResult> GetResponse()
    {
        await Task.Delay(250);
        return RegisterResult.Success;
    }

    public void Dispose() { }
}