using Microsoft.Data.SqlClient;
using WhatsDown.Core.CommunicationProtocol.Enums;
using WhatsDown.Core.Interfaces;
using WhatsDown.Core.Models;
using WhatsDown.Server.Interfaces.Services;
using WhatsDown.Server.Interfaces.Services.Database;

namespace WhatsDown.Server.Services.Database.SqlDirect;

internal class SqlDbHandler : IDatabaseHandler
{
    private readonly ILogger logger;

    private readonly SqlConnection conn;

    public SqlDbHandler(IConfigurationFetcher configFetcher, ILogger logger)
    {
        this.logger = logger;

		string connString = configFetcher.GetStringAttribute("Database:Sql:ConnectionStrings:Default");
		logger.LogInformation($"Started SQL Conn: {connString}");
		conn = new SqlConnection(connString);
    }

	public Task<ChatModel> GetChat(int chatId)
	{
		throw new NotImplementedException();
	}

	public Task<IEnumerable<ChatModel>> GetChatHistory(int userId)
	{
		throw new NotImplementedException();
	}

	public Task<IEnumerable<MessageModel>> GetMessages(int chatId)
	{
		throw new NotImplementedException();
	}

	public Task<UserModel> GetUser(int userId)
	{
		throw new NotImplementedException();
	}

	public Task<int> GetUserId(string email, string password)
	{
		throw new NotImplementedException();
	}

	public Task<LoginResult> VerifyLogin(string email, string password)
	{
		throw new NotImplementedException();
	}

	public Task<RegisterResult> VerifyRegister(string email, string password)
	{
		throw new NotImplementedException();
	}
}