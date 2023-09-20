using System;
using System.Diagnostics;
using System.Net;
using System.Threading.Tasks;
using WhatsDown.Core.CommunicationProtocol;
using WhatsDown.Core.CommunicationProtocol.Enums;
using WhatsDown.Core.Exceptions;
using WhatsDown.Core.Interfaces;
using WhatsDown.Core.Models;
using WhatsDown.Core.NetworkingShared;
using WhatsDown.WPF.Interfaces;
using WhatsDown.WPF.Interfaces.Aliases;

namespace WhatsDown.WPF.DI.Services.RequestResponseServices;

internal class RegisterRequestService : IRegisterRequest
{
    private readonly INetworkClient client;
    private readonly ILogger logger;

    private readonly Stopwatch timeoutStopwatch = new Stopwatch();

    public RegisterRequestService(INetworkClient client, ILogger logger)
    {
        this.client = client;
        this.logger = logger;
    }

    public async Task<RegisterResult> Procedure(CredentialsModel model)
    {
        logger.LogInformation($"Started Register Procedure: ({model.Email}, {model.Password})");
        try
        {
            if (!await client.Connect(IPAddress.Parse("127.0.0.1"), 7777))
                return RegisterResult.ServerUnreachable;
            await PostRequest(model);
            MessagePacket response = await GetResponse();

            RegisterResult result =  response.ExtractParamAsEnum<RegisterResult>(0);
            client.Disconnect();
            return result;
        }
        catch (Exception ex)
        {
            logger.LogInformation($"Error Occured in Register: {ex.Message}");
            return RegisterResult.UnknownError;
        }
    }

    public void TerminateProcedure()
    {
        timeoutStopwatch.Reset();
        client.Disconnect();
    }

    public async Task PostRequest(CredentialsModel model)
    {
        MessagePacket loginPacket = new MessagePacket(CommunicationType.LoginRequest, model.Email, model.Password);
        await client.WriteMessage(loginPacket);
    }

    public async Task<MessagePacket> GetResponse()
    {
        try
        {
            timeoutStopwatch.Restart();
			MessagePacket? response = await client.ReadMessage();
			if (response == null)
				return new MessagePacket(CommunicationType.RegisterResponse, RegisterResult.UnknownError);

			return response;
		}
		catch (TimeoutException) { logger.LogError("Register Request Timedout Exception"); }
        catch (NetworkedReadException) { logger.LogError("Register Request Read Exception"); }
        catch (Exception ex) { logger.LogFatal($"MIS-HANDLED: {ex.Message}"); }
		finally { TerminateProcedure(); }
		return new MessagePacket(CommunicationType.RegisterResponse, RegisterResult.Timeout);
    }

    public int GetTimeoutPercentage()
    {
        return (int)(timeoutStopwatch.Elapsed.TotalSeconds / NetworkingConstants.ReadTimeoutSeconds* 100);
    }
}