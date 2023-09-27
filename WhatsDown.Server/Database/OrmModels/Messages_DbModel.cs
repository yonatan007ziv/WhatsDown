using WhatsDown.Core.Models;

namespace WhatsDown.Server.Database.OrmModels;

internal class Messages_DbModel
{
	public int MessageId { get; set; }
	public DateTime SentTimestamp { get; set; }
	public string content { get; set; } = null!;
	public long FileSize { get; set; }
	public MessageType MessageType { get; set; }
	public int ChatId { get; set; }
	public int UserId { get; set; }
}