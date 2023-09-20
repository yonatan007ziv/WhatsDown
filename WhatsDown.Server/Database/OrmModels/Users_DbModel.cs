namespace WhatsDown.Server.Database.OrmModels;

internal class Users_DbModel
{
    public int UserId { get; set; }
    public string DisplayName { get; set; } = null!;
    public string Description { get; set; } = null!;
    public DateTime CreatedTimestamp { get; set; }
    public string ProfileImageUrl { get; set; } = null!;
    public IEnumerable<Chats_DbModel> Chats { get; set; } = new List<Chats_DbModel>(); // Junction Table, Many-to-Many
}