using System.Threading.Tasks;
using WhatsDown.Core.CommunicationProtocol.Enums;
using WhatsDown.Core.Interfaces;
using WhatsDown.WPF.Interfaces.RequestResponse;
using WhatsDown.WPF.MVVM.Models;

namespace WhatsDown.WPF.Mockers;

internal class MockLoginHandlerService : ILoginRequestResponseHandler
{
    private readonly ILogger logger;

    private string Email { get; set; } = "";
    private string Password { get; set; } = "";

    public MockLoginHandlerService(ILogger logger)
    {
        this.logger = logger;
    }

    public async Task<LoginResult> LoginProcedure(LoginModel model)
    {
        logger.LogInformation($"LOGIN MOCK: email({Email}) password({Password})");
        await PostRequest(model);
        return await GetResponse();
    }

    public Task PostRequest(LoginModel model)
    {
        Email = model.Email;
        Password = model.Password;
        return Task.CompletedTask;
    }

    public async Task<LoginResult> GetResponse()
    {
        await Task.Delay(250);
        return LoginResult.Success;
    }


    public void Dispose() { }
}