using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WhatsDown.Server.Database.Models;

public class UserInformation
{
    [Key]
    public int UserId { get; set; }

    [Required]
    [MaxLength(100)]
    public string DisplayName { get; set; } = null!;

    [Required]
    [MaxLength(100)]
    [Column(TypeName = "VARCHAR(100)")]
    public string ImagePath { get; set; } = null!;

    [MaxLength(250)]
    public string Description { get; set; } = null!;
}