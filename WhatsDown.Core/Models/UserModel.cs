namespace WhatsDown.Core.Models;

public class UserModel
{
	public int DisplayName { get; set; }
	public int Description { get; set; }
	public DateTime CreatedTimestamp { get; set; }
	public string ProfileImageUrl { get; set; } = null!;
}