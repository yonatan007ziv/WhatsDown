using WhatsDown.Core.Models;

namespace WhatsDown.Server.Interfaces.Services.Database;

// Revise as needed
internal interface IDatabaseAnalyzer
{
    Task<CredentialsModel> GetUserCredentials(int userId);
    Task<IEnumerable<ChatModel>> GetUserChats(int userId);
    Task<IEnumerable<MessageModel>> GetChatModel(int chatId);
    Task<bool> IsUserInChat(int userId, int chatId);
    Task<IEnumerable<UserModel>> GetChatParticipants(int chatId);
    Task<int> GetUnreadMessagesCount(int userId, int chatId);
    Task MarkMessagesAsRead(int userId, int chatId);
    Task<bool> IsUserBlocked(int userId, int blockedUserId);
    Task<IEnumerable<MessageModel>> SearchMessagesInChat(int chatId, string searchTerm);
    Task ReportInappropriateContent(int messageId);
}