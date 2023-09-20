using Microsoft.Data.Sqlite;
using WhatsDown.Core.CommunicationProtocol.Enums;
using WhatsDown.Core.Interfaces;
using WhatsDown.Core.Models;
using WhatsDown.Server.Interfaces.Services;
using WhatsDown.Server.Interfaces.Services.Database;
using WhatsDown.Server.Interfaces.Services.Security;

namespace WhatsDown.Server.Services.Database.SqlDirect;

internal class SqLiteDbHandler : IDatabaseHandler
{
	private readonly IHasher hasher;
	private readonly ISalter salter;
	private readonly ILogger logger;

	private readonly SqliteConnection conn;

	public SqLiteDbHandler(IHasher hasher, ISalter salter,IConfigurationFetcher configFetcher, ILogger logger)
	{
		this.hasher = hasher;
		this.salter = salter;
		this.logger = logger;

		string connString = configFetcher.GetStringAttribute("Database:SqLite:ConnectionStrings:Default");
		logger.LogInformation($"Started SQLITE Conn: {connString}");
		conn = new SqliteConnection(connString);
	}

	public Task<IEnumerable<ChatModel>> GetChatHistory(int userId)
	{
		return Task.FromResult(GetChatHistoryTemp(userId));
	}

	public Task<ChatModel> GetChat(int chatId)
	{
		return Task.FromResult(GetChatTemp(chatId));
	}

	public Task<UserModel> GetUser(int userId)
	{
		throw new NotImplementedException();
	}

	public Task<LoginResult> VerifyLogin(string email, string password)
	{
		return Task.FromResult(LoginResult.Success);
	}

	public Task<RegisterResult> VerifyRegister(string email, string password)
	{
		throw new NotImplementedException();
	}
	
	private IEnumerable<ChatModel> GetChatHistoryTemp(int chathistorySuffix)
	{
		return new List<ChatModel>
		{
			GetChatTemp(1),
			GetChatTemp(4),
			GetChatTemp(7),
		};
	}
	private ChatModel GetChatTemp(int chatSuffix)
	{
		return new ChatModel
		{
			Title = $"TITLE{chatSuffix}",
			Description = $"DESCRIPTION{chatSuffix}",
			Messages = new List<MessageModel>
			{
				GetMessageTemp(chatSuffix),
				GetMessageTemp(chatSuffix + 1),
				GetMessageTemp(chatSuffix + 2),
			}
		};
	}
	private MessageModel GetMessageTemp(int messageSuffix)
	{
		return new MessageModel
		{
			DisplayName = $"DISPLAYNAME{messageSuffix}",
			Content = $"CONTENT{messageSuffix}",
			FileSize = -1,
			MessageType = 0,
			Timestamp = DateTime.Now + TimeSpan.FromSeconds(messageSuffix)
		};
	}

	public Task<int> GetUserId(string email, string password)
	{
		return Task.FromResult(1);
	}
}