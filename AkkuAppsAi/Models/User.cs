using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AkkuAppsAi.Models;

[Table("Users")]
public class User
{
    [Key]
    [MaxLength(36)]
    public string UserId { get; set; } = null!;

    [MaxLength(255)]
    public string? GoogleId { get; set; }

    [Required]
    [MaxLength(255)]
    public string Email { get; set; } = null!;

    [Required]
    [MaxLength(64)]
    public string EmailHash { get; set; } = null!;

    [Required]
    [MaxLength(100)]
    public string Name { get; set; } = null!;

    [MaxLength(64)]
    public string? PasswordHash { get; set; }

    [MaxLength(500)]
    public string? Avatar { get; set; }

    public string? Bio { get; set; }

    public decimal CoinBalance { get; set; }

    public decimal WalletBalance { get; set; }

    [MaxLength(20)]
    public string UserType { get; set; } = "AkkuUser";

    [MaxLength(20)]
    public string Role { get; set; } = "user";

    public bool IsVerified { get; set; }

    public bool IsBanned { get; set; }

    public DateTime? LastLogin { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }
}
