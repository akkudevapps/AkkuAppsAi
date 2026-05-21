using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AkkuAppsAi.Models;

[Table("CoinPurchaseOrders")]
public class CoinPurchaseOrder
{
    [Key]
    [MaxLength(36)]
    public string OrderId { get; set; } = Guid.NewGuid().ToString();

    [Required]
    [MaxLength(36)]
    public string UserId { get; set; } = null!;

    public int? PackageId { get; set; }

    public decimal Coins { get; set; }

    public decimal AmountINR { get; set; }

    [MaxLength(100)]
    public string? UpiTransactionId { get; set; }

    [MaxLength(500)]
    public string? UpiQrUrl { get; set; }

    [MaxLength(20)]
    public string Status { get; set; } = "Pending";

    [MaxLength(36)]
    public string? ApprovedBy { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime? ApprovedAt { get; set; }
}
