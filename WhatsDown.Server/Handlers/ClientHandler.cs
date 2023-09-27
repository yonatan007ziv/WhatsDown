using WhatsDown.Core.CommunicationProtocol;
using WhatsDown.Core.CommunicationProtocol.Enums;
using WhatsDown.Core.Exceptions;
using WhatsDown.Core.Interfaces;
using WhatsDown.Core.Interfaces.Networking;
using WhatsDown.Server.Interfaces.Services;
using WhatsDown.Server.Interfaces.Services.Database;

namespace WhatsDown.Server.Handlers;

internal class ClientHandler : IClientHandler
{
	private static readonly Dictionary<string, int> TokenToIdMapper = new Dictionary<string, int>();
	private static readonly List<(string token, string email, string password)> TwoFARegisterTokens = new List<(string, string, string)>();

	private readonly INetworkClientHandler client;
	private readonly IDatabaseAnalyzer databaseHandler;
	private readonly IEmailSender emailSender;
	private readonly ITokenGenerator<string> tokenGenerator;
	private readonly ISerializer serializer;
	private readonly ILogger logger;

	private bool userValidated;
	private int userId = -1;

	public ClientHandler(INetworkClientHandler client, IDatabaseAnalyzer databaseHandler, IEmailSender emailSender, ITokenGenerator<string> tokenGenerator, ISerializer serializer, ILogger logger)
	{
		this.client = client;
		this.databaseHandler = databaseHandler;
		this.emailSender = emailSender;
		this.tokenGenerator = tokenGenerator;
		this.serializer = serializer;
		this.logger = logger;
	}

	public async void Start()
	{
		try
		{
			await ReadMessageLoop();
		}
		catch (Exception ex)
		{
			logger.LogError($"Error Occured in Client: {ex.Message}");
			Disconnect();
		}
	}

	public async Task ReadMessageLoop()
	{
		while (true)
		{
			MessagePacket received;
			try
			{
				received = await client.ReadMessage();
				if (received.InvalidPacket)
					break;
			}
			catch (NetworkedReadException) { break; }

			// Validated Token Not Needed
			switch (received.Type)
			{
				case CommunicationType.TokenValidationRequest:
					HandleTokenValidationRequest(received);
					break;
				case CommunicationType.LoginRequest:
					HandleLoginRequest(received);
					break;
				case CommunicationType.RegisterRequest:
					HandleRegisterRequest(received);
					break;
			}

			// Validated Token Needed
			if (!userValidated)
				continue;

			switch (received.Type)
			{
				case CommunicationType.ChatsRequest:
					HandleChatsRequest();
					break;
			}
		}

		logger.LogWarning("Exited Client's Read-Loop" + (userValidated ? $", UserID: {userId}" : "."));
		Disconnect();
	}

	private void HandleTokenValidationRequest(MessagePacket context)
	{
		if (TokenToIdMapper.ContainsKey(context.Params[0]))
		{
			userValidated = true;
			userId = TokenToIdMapper[context.Params[0]];
			_ = client.WriteMessage(new MessagePacket(CommunicationType.TokenValidationResponse, TokenResult.Success));
		}
		else
			_ = client.WriteMessage(new MessagePacket(CommunicationType.TokenValidationResponse, TokenResult.Error));
	}

	private async void HandleLoginRequest(MessagePacket context)
	{
		// Captcha ?

		if (context.Params.Length < 2)
			_ = client.WriteMessage(new MessagePacket(CommunicationType.LoginResponse, LoginResult.InvalidCredentials));

		string email = context.Params[0], password = context.Params[1];
		LoginResult loginResult = await databaseHandler.VerifyLogin(email, password);
		if (loginResult == LoginResult.Success)
		{
			userId = await databaseHandler.GetUserId(email, password);

			string token = tokenGenerator.GenerateUniqueToken();
			TokenToIdMapper.Add(token, userId);
			_ = client.WriteMessage(new MessagePacket(CommunicationType.LoginResponse, LoginResult.Success, token));
			return;
		}

		_ = client.WriteMessage(new MessagePacket(CommunicationType.LoginResponse, loginResult));
	}

	private async void HandleRegisterRequest(MessagePacket context)
	{
		// Captcha ?

		if (context.Params.Length < 2)
			_ = client.WriteMessage(new MessagePacket(CommunicationType.RegisterResponse, RegisterResult.InvalidCredentials));

		string email = context.Params[0], password = context.Params[1];
		RegisterResult registerResult = await databaseHandler.VerifyRegister(email, password);

		if (registerResult == RegisterResult.TwoFANeeded)
		{
			string token = tokenGenerator.GenerateUniqueToken();
			//TwoFARegisterTokens.Add(token);

			await emailSender.SendMessage(email, "2FA Authentication", token);
			_ = client.WriteMessage(new MessagePacket(CommunicationType.RegisterResponse, RegisterResult.TwoFANeeded, token));
			return;
		}

		_ = client.WriteMessage(new MessagePacket(CommunicationType.RegisterResponse, registerResult));
	}

	private async void HandleChatsRequest()
	{
		string serialized = serializer.Serialize(await databaseHandler.GetChatHistory(userId)) ?? "";
		_ = client.WriteMessage(new MessagePacket(CommunicationType.ChatsRequest, serialized));
	}

	private void Disconnect()
	{
		client.Disconnect();
	}
}