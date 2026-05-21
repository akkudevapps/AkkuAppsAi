using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AkkuAppsAi.Models;

[Table("news_blogs")]
public class NewsBlog
{
    [Key]
    [MaxLength(36)]
    public string BlogId { get; set; } = Guid.NewGuid().ToString();

    [Required]
    [MaxLength(36)]
    public string AuthorId { get; set; } = null!;

    [Required]
    [MaxLength(255)]
    public string Title { get; set; } = null!;

    [Required]
    [MaxLength(255)]
    public string Slug { get; set; } = null!;

    public string? Excerpt { get; set; }

    public string? Content { get; set; }

    [MaxLength(100)]
    public string? Category { get; set; } = "general";

    [Required]
    [MaxLength(20)]
    public string ArticleType { get; set; } = "news";

    [MaxLength(500)]
    public string? FeaturedImage { get; set; }

    [Required]
    [MaxLength(20)]
    public string Status { get; set; } = "published";

    public DateTime? PublishedAt { get; set; } = DateTime.UtcNow;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}
