using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AkkuAppsAi.Models;

[Table("DigitalGoods")]
public class DigitalGood
{
    [Key]
    [MaxLength(36)]
    public string GoodId { get; set; } = Guid.NewGuid().ToString();

    [Required]
    [MaxLength(255)]
    public string Title { get; set; } = null!;

    [MaxLength(2000)]
    public string? Description { get; set; }

    [Required]
    public decimal PriceInCoins { get; set; }

    public bool HasCommission { get; set; }

    [MaxLength(100)]
    public string? Category { get; set; }

    [MaxLength(500)]
    public string? ThumbnailUrl { get; set; }

    [MaxLength(500)]
    public string? FileUrl { get; set; }

    [Required]
    [MaxLength(36)]
    public string CreatedByUserId { get; set; } = null!;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public bool IsActive { get; set; } = true;
}
