namespace WhatsDown.Server.Database.OrmModels;

internal class Chats_DbModel
{
    public int ChatId { get; set; }
    public string Title { get; set; } = null!;
	public string Description{ get; set; } = null!;
	public DateTime CreatedTimestamp { get; set; }
    public string ChatImageUrl { get; set; } = null!;
	public IEnumerable<Users_DbModel> Chats { get; set; } = new List<Users_DbModel>(); // Junction Table, Many-to-Many
}