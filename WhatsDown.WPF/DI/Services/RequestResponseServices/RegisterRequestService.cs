using System.Diagnostics;
using System.Net;
using System.Threading.Tasks;
using WhatsDown.Core.CommunicationProtocol;
using WhatsDown.Core.CommunicationProtocol.Enums;
using WhatsDown.Core.Interfaces;
using WhatsDown.Core.Models;
using WhatsDown.WPF.Interfaces;
using WhatsDown.WPF.Interfaces.Aliases;

namespace WhatsDown.WPF.DI.Services.RequestResponseServices;

internal class RegisterRequestService : IRegisterRequest
{
	private readonly INetworkClient client;

	private readonly IPAddress serverIp;
	private readonly int serverPort;

	private readonly int responseTimeout;
	private readonly Stopwatch timeoutStopwatch = new Stopwatch();

	public RegisterRequestService(INetworkClient client, IConfigurationFetcher configFetcher)
	{
		this.client = client;

		serverIp = IPAddress.Parse(configFetcher.GetStringAttribute("ServerEndpoint:Ip"));
		serverPort = configFetcher.GetIntAttribute("ServerEndpoint:Port");

		responseTimeout = configFetcher.GetIntAttribute("Timeouts:ReadTimeout");
	}

	public async Task<RegisterResult> Procedure(CredentialsModel model)
	{
		if (!await client.Connect(serverIp, serverPort))
			return RegisterResult.ServerUnreachable;

		await PostRequest(model);

		MessagePacket response = await GetResponse();
		if (response.InvalidPacket)
		{
			timeoutStopwatch.Reset();
			return RegisterResult.UnknownError;
		}
		else if (response.TimedOut)
		{
			timeoutStopwatch.Reset();
			return RegisterResult.Timeout;
		}

		RegisterResult result = response.ExtractParamAsEnum<RegisterResult>(0);

		if (result == RegisterResult.TwoFANeeded)
			client.IntegrityToken = response.Params[1];

		client.Disconnect();
		timeoutStopwatch.Reset();
		return result;
	}


	public void StopProcedure()
	{
		timeoutStopwatch.Reset();
		client.Disconnect();
	}

	public async Task PostRequest(CredentialsModel model)
	{
		MessagePacket loginPacket = new MessagePacket(CommunicationType.RegisterRequest, model.Email, model.Password);
		await client.WriteMessage(loginPacket);
	}

	public async Task<MessagePacket> GetResponse()
	{
		timeoutStopwatch.Restart();
		return await client.ReadMessage();
	}

	public int GetTimeoutPercentage()
	{
		return (int)(timeoutStopwatch.Elapsed.TotalSeconds / responseTimeout * 100);
	}
}