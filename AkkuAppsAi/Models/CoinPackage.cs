using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AkkuAppsAi.Models;

[Table("CoinPackages")]
public class CoinPackage
{
    [Key]
    public int PackageId { get; set; }

    [Required]
    public decimal Coins { get; set; }

    [Required]
    public decimal PriceINR { get; set; }

    [MaxLength(100)]
    public string? Description { get; set; }

    public bool IsActive { get; set; } = true;
}
