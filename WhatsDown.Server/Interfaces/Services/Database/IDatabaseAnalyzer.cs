using WhatsDown.Core.Models;

namespace WhatsDown.Server.Interfaces.Services.Database;

internal interface IDatabaseAnalyzer
{
    List<ChatModel> GetUserChats(int userId);
    List<MessageModel> GetChatMessages(int chatId);
    bool IsUserInChat(int userId, int chatId);
    List<UserModel> GetChatParticipants(int chatId);
    int GetUnreadMessagesCount(int userId, int chatId);
    void MarkMessagesAsRead(int userId, int chatId);
    bool IsUserBlocked(int userId, int blockedUserId);
    List<MessageModel> SearchMessagesInChat(int chatId, string searchTerm);
    void ReportInappropriateContent(int messageId);
}