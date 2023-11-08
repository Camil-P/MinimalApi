using Microsoft.AspNetCore.Mvc;
using MinimalApiTemplate.Api.Entities.Enums;
using System.Web;

namespace MinimalApiTemplate.Api.Contracts.Posts;

public class GetPostsRequest
{
    public string? Description { get; set; }
    public VisibilityStatus? VisibilityStatus { get; set; }
    public List<string>? Tags { get; set; }

    public static bool TryParse(string query, out GetPostsRequest result)
    {
        var parsedQuery = HttpUtility.ParseQueryString(query);

        result = new GetPostsRequest
        {
            Description = parsedQuery["Description"],
            VisibilityStatus = Enum.TryParse(parsedQuery["VisibilityStatus"], out VisibilityStatus status) ? status : null,
            Tags = parsedQuery.GetValues("Tags")?.ToList()
        };

        return true; // Return true if parsing is successful.
    }
}
