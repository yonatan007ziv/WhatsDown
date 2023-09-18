namespace WhatsDown.Core.Models;

public class MessageModel
{
    public int MessageId { get; set; }
    public int ChatId { get; set; }
    public int SenderId { get; set; }
    public string Content { get; set; } = null!;
    public DateTime Timestamp { get; set; }
    public bool IsRead { get; set; }
    public MessageContent MessageContent { get; set; }
}