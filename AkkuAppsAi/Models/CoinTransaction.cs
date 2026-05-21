using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AkkuAppsAi.Models;

[Table("CoinTransactions")]
public class CoinTransaction
{
    [Key]
    [MaxLength(36)]
    public string TxnId { get; set; } = null!;

    [Required]
    [MaxLength(36)]
    public string UserId { get; set; } = null!;

    [Required]
    [MaxLength(50)]
    public string ReferenceType { get; set; } = null!;

    public decimal Amount { get; set; }

    public decimal BalanceAfter { get; set; }

    [MaxLength(255)]
    public string? Description { get; set; }

    public DateTime CreatedAt { get; set; }
}
