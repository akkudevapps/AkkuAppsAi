using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AkkuAppsAi.Models;

[Table("Purchases")]
public class Purchase
{
    [Key]
    [MaxLength(36)]
    public string PurchaseId { get; set; } = Guid.NewGuid().ToString();

    [Required]
    [MaxLength(36)]
    public string UserId { get; set; } = null!;

    [Required]
    [MaxLength(36)]
    public string GoodId { get; set; } = null!;

    public decimal CoinsPaid { get; set; }

    public decimal AdminCommission { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
