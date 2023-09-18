using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WhatsDown.Server.Database.Models;

public class AccountCredentials
{
    [Key]
    public int UserId { get; set; }

    [Required]
    [MaxLength(100)]
    public string Email { get; set; } = null!;

    [Required]
    [MaxLength(32)]
    [Column(TypeName = "VARCHAR(32)")]
    public string PasswordHash { get; set; } = null!;

    [Required]
    [MaxLength(50)]
    [Column(TypeName = "VARCHAR(50)")]
    public string PasswordSalt { get; set; } = null!;

    [Required]
    public UserInformation userInformation { get; set; } = null!;
}