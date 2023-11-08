using MinimalApiTemplate.Api.Entities.Enums;

namespace MinimalApiTemplate.Api.Contracts.Posts;

public class GetPostsResponse
{
    public Guid Id { get; set; }
    public string Description { get; set; } = null!;
    public string? Image { get; set; }
    public VisibilityStatus VisibilityStatus { get; set; }

    public List<string> Tags { get; set; } = new();
    public DateTime UpdatedAt { get; set; }
    public DateTime CreatedAt { get; set; }
}
