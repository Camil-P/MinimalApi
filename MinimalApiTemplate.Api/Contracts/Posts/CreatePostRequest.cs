using MinimalApiTemplate.Api.Entities.Enums;

namespace MinimalApiTemplate.Api.Contracts.Posts;

public class CreatePostRequest
{
    public string Description { get; set; } = null!;
    public string? Image { get; set; }
    public VisibilityStatus VisibilityStatus { get; set; }
    public List<string>? Tags { get; set; }
}
