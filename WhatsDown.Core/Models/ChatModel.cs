namespace WhatsDown.Core.Models;

public class ChatModel
{
    public string Title { get; set; } = null!;
	public string Description { get; set; } = null!;
	public DateTime CreatedTimestamp { get; set; }
	public string ChatImageUrl { get; set; } = null!;
	public IEnumerable<UserModel> Participants { get; set; } = null!;
	public IEnumerable<MessageModel> Messages { get; set; } = null!;
}