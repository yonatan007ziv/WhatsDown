namespace WhatsDown.Core.Models;

public class MessageModel
{
	public string DisplayName { get; set; } = null!;
	public string Content { get; set; } = null!;
	public long FileSize { get; set; }
	public DateTime Timestamp { get; set; }
	public MessageType MessageType { get; set; }
}

public enum MessageType
{
	Text,
	Image,
	Mp3
}