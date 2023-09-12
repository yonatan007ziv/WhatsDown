namespace WhatsDown.Server.Database.Models;

internal class AccountCredentials : BaseTable
{
    public string DisplayName { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string PasswordHash { get; set; } = null!;
    public string PasswordSalt { get; set; } = null!;
}