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

	private readonly INetworkCommunication client;
	private readonly IDatabaseHandler databaseHandler;
	private readonly ITokenGenerator<string> tokenGenerator;
	private readonly IChatUserMessageSerializer chatUserMessageSerializer;
	private readonly ILogger logger;

	private int UserId = -1;

	public ClientHandler(INetworkCommunication client, IDatabaseHandler databaseHandler, ITokenGenerator<string> tokenGenerator, IChatUserMessageSerializer chatUserMessageSerializer, ILogger logger)
	{
		this.client = client;
		this.databaseHandler = databaseHandler;
		this.tokenGenerator = tokenGenerator;
		this.chatUserMessageSerializer = chatUserMessageSerializer;
		this.logger = logger;
	}

	public async Task ReadMessageLoop()
	{
		while (true)
		{
			MessagePacket received;
			try
			{
				received = await client.ReadMessage();
				if (received.Result == CommunicationValid.No)
					break;
			}
			catch (NetworkedReadException){ break; }

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
			if (UserId < 0)
				continue;

			switch (received.Type)
			{
				case CommunicationType.ChatsRequest:
					HandleChatsRequest();
					break;
			}
		}
	}

	private void HandleTokenValidationRequest(MessagePacket context)
	{
		if (TokenToIdMapper.ContainsKey(context.Params[0]))
		{
			UserId = TokenToIdMapper[context.Params[0]];
			_ = client.WriteMessage(new MessagePacket(CommunicationType.TokenValidationResponse, TokenResult.Success));
		}
		else
			_ = client.WriteMessage(new MessagePacket(CommunicationType.TokenValidationResponse, TokenResult.Error));
	}

	private async void HandleLoginRequest(MessagePacket context)
	{
		if (context.Params.Length < 2)
			_ = client.WriteMessage(new MessagePacket(CommunicationType.LoginResponse, LoginResult.InvalidCredentials));

		string email = context.Params[0], password = context.Params[1];
		LoginResult loginResult = await databaseHandler.VerifyLogin(email, password);
		if (loginResult == LoginResult.Success)
		{
			UserId = await databaseHandler.GetUserId(email, password);

			string token = tokenGenerator.GenerateUniqueToken();
			TokenToIdMapper.Add(token, UserId);
			_ = client.WriteMessage(new MessagePacket(CommunicationType.LoginResponse, loginResult, token));
			return;
		}

		_ = client.WriteMessage(new MessagePacket(CommunicationType.LoginResponse, loginResult));
	}

	private void HandleRegisterRequest(MessagePacket context)
	{
		if (context.Params.Length < 2)
			_ = client.WriteMessage(new MessagePacket(CommunicationType.RegisterResponse, RegisterResult.InvalidCredentials));

		logger.LogInformation($"Got Register Request, Context Params: {context.Params[0]},{context.Params[1]}");
	}

	private async void HandleChatsRequest()
	{
		_ = client.WriteMessage(new MessagePacket(CommunicationType.ChatsRequest, chatUserMessageSerializer.Serialize(await databaseHandler.GetChatHistory(UserId))!));
	}
}