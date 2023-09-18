using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using System.Data;
using System.Linq.Expressions;
using WhatsDown.Core.Models;
using WhatsDown.Server.Interfaces.Services.Database;

namespace WhatsDown.Server.Services.Database;

internal class DatabaseAnalyzer : IDatabaseAnalyzer
{
    private readonly IDatabaseExtractor extractor;

    public DatabaseAnalyzer(IDatabaseExtractor extractor)
    {
        this.extractor = extractor;
    }

    public List<MessageModel> GetChatMessages(int chatId)
    {
        throw new NotImplementedException();
    }

    public List<UserModel> GetChatParticipants(int chatId)
    {
        throw new NotImplementedException();
    }

    private DataTable GetChatAsTable(int chatId)
    {
        throw new NotImplementedException();
    }

    public int GetUnreadMessagesCount(int userId, int chatId)
    {
        throw new NotImplementedException();
    }

    public List<ChatModel> GetUserChats(int userId)
    {
        throw new NotImplementedException();
    }

    public bool IsUserBlocked(int userId, int blockedUserId)
    {
        throw new NotImplementedException();
    }

    public bool IsUserInChat(int userId, int chatId)
    {
        throw new NotImplementedException();
    }

    public void MarkMessagesAsRead(int userId, int chatId)
    {
        throw new NotImplementedException();
    }

    public void ReportInappropriateContent(int messageId)
    {
        throw new NotImplementedException();
    }

    public List<MessageModel> SearchMessagesInChat(int chatId, string searchTerm)
    {
        throw new NotImplementedException();
    }
}