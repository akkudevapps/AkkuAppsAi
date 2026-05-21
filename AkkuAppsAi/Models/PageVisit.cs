using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AkkuAppsAi.Models;

[Table("PageVisits")]
public class PageVisit
{
    [Key]
    public long VisitId { get; set; }

    [MaxLength(45)]
    public string? IPAddress { get; set; }

    [MaxLength(100)]
    public string? SessionId { get; set; }

    [MaxLength(500)]
    public string? Url { get; set; }

    [MaxLength(500)]
    public string? UserAgent { get; set; }

    [MaxLength(100)]
    public string? Country { get; set; }

    [MaxLength(100)]
    public string? City { get; set; }

    [MaxLength(64)]
    public string? TimeZone { get; set; }

    public DateTime VisitedAt { get; set; } = DateTime.UtcNow;
}
