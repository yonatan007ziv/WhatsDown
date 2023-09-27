using WhatsDown.Core.CommunicationProtocol.Enums;
using WhatsDown.Core.Models;

namespace WhatsDown.Server.Interfaces.Services.Database;

// Revise as needed
internal interface IDatabaseAnalyzer
{
	Task<LoginResult> VerifyLogin(string email, string password);
	Task<RegisterResult> VerifyRegister(string email, string password);

	Task<IEnumerable<ChatModel>> GetChatHistory(int userId);
	Task<ChatModel> GetChat(int chatId);
	Task<UserModel> GetUser(int userId);
	Task<int> GetUserId(string email, string password);
}