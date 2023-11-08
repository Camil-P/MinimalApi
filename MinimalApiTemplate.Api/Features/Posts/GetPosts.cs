using Carter;
using FluentValidation;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MinimalApiTemplate.Api.Contracts.Posts;
using MinimalApiTemplate.Api.Database;
using MinimalApiTemplate.Api.Entities.Enums;

namespace MinimalApiTemplate.Api.Features.Posts;

public class GetPosts
{
    public class Query : IRequest<List<GetPostsResponse>>
    {
        public string? Description { get; set; }
        public VisibilityStatus? VisibilityStatus { get; set; }
        public List<string>? Tags { get; set; }
    }

    internal sealed class Handler : IRequestHandler<Query, List<GetPostsResponse>>
    {
        private readonly DataContext _dbContext;

        public Handler(DataContext dbContext)
        {
            _dbContext = dbContext;
        }
        public Task<List<GetPostsResponse>> Handle(Query request, CancellationToken cancellationToken)
        {
            try
            {
                var posts = _dbContext.Posts.AsQueryable();

                if (!string.IsNullOrEmpty(request.Description))
                {
                    posts = posts.Where(p => p.Description.ToLower().Contains(request.Description.ToLower()));
                }

                if (request.VisibilityStatus.HasValue)
                {
                    posts = posts.Where(p => p.VisibilityStatus.Equals(request.VisibilityStatus));
                }

                if (request.Tags is not null && request.Tags.Count > 0)
                {
                    posts = posts.Where(p => request.Tags.Any(t => p.Tags.Contains(t)));
                }

                return posts.Select(p => p.Adapt<GetPostsResponse>()).ToListAsync(cancellationToken);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}

public class GetPostsEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("api/posts", async ([FromQuery] string? description,
                                       [FromQuery] VisibilityStatus? visibilityStatus,
                                       [FromQuery] string[] tags,
                                       ISender sender) =>
        {
            var query = new GetPosts.Query
            {
                Description = description,
                VisibilityStatus = visibilityStatus,
                Tags = new List<string>(tags),
            };

            var res = await sender.Send(query);

            return Results.Ok(res);
        });
    }
}