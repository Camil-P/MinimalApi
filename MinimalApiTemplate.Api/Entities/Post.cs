using MinimalApiTemplate.Api.Entities.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace MinimalApiTemplate.Api.Entities;

public class Post : BaseEntity
{
    public string Description { get; set; } = null!;
    public string? Image { get; set; }
    public VisibilityStatus VisibilityStatus { get; set; }

    public List<string> Tags { get; set; } = new();

    //[ForeignKey("AuthorId")]
    //public User Author { get; set; } = null!;
    //public Guid AuthorId { get; set; }
}
