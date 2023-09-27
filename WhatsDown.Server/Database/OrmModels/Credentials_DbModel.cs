namespace WhatsDown.Server.Database.OrmModels;

public class Credentials_DbModel
{
	public int AccountCredentialsId { get; set; }
	public string Email { get; set; } = null!;
	public string PasswordHash { get; set; } = null!;
	public string PasswordSalt { get; set; } = null!;
	public int UserId { get; set; }
}