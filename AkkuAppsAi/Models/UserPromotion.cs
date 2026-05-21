using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AkkuAppsAi.Models;

[Table("UserPromotions")]
public class UserPromotion
{
    [Key]
    [MaxLength(36)]
    public string PromotionId { get; set; } = Guid.NewGuid().ToString();

    [Required]
    [MaxLength(36)]
    public string UserId { get; set; } = null!;

    [MaxLength(20)]
    public string NewUserType { get; set; } = null!;

    [MaxLength(100)]
    public string? PlanName { get; set; }

    public decimal CoinsCharged { get; set; }

    [MaxLength(36)]
    public string? ApprovedBy { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
