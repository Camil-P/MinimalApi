using Carter;
using FluentValidation;
using Mapster;
using MediatR;
using MinimalApiTemplate.Api.Contracts.Posts;
using MinimalApiTemplate.Api.Database;
using MinimalApiTemplate.Api.Entities;
using MinimalApiTemplate.Api.Entities.Enums;

namespace MinimalApiTemplate.Api.Features.Posts;

public static class CreatePost
{
    public class Command : IRequest<Guid>{
        public string Description { get; set; } = null!;
        public string? Image { get; set; }
        public VisibilityStatus VisibilityStatus { get; set; }
        public List<string>? Tags { get; set; }
    }

    public class Validator : AbstractValidator<Command>
    {
        public Validator()
        {
            RuleFor(x => x.Description).NotEmpty();
            RuleFor(x => x.Image).NotEmpty();
            RuleFor(x => x.VisibilityStatus).NotEmpty();
        }
    }

    internal sealed class Handler : IRequestHandler<Command, Guid>
    {
        private readonly DataContext _dbContext;
        private readonly IValidator<Command> _validator;

        public Handler(DataContext dbContext, IValidator<Command> validator)
        {
            _dbContext = dbContext;
            _validator = validator;
        }

        public async Task<Guid> Handle(Command request, CancellationToken cancellationToken)
        {
            try
            {
                var validationResult = _validator.Validate(request);
                if (!validationResult.IsValid)
                {
                    throw new BadHttpRequestException(validationResult.Errors[0].ErrorMessage, 400);
                }

                Post post = new()
                {
                    Description = request.Description,
                    Image = request.Image,
                    VisibilityStatus = request.VisibilityStatus,
                    Tags = request?.Tags ?? new List<string>(),
                };

                await _dbContext.AddAsync(post, cancellationToken);

                await _dbContext.SaveChangesAsync(cancellationToken);

                return post.Id;
            }
            catch (Exception ex)
            {
                var inner = ex.InnerException;
                throw;
            }
        }
    }

    //public static void MapEndpoint(this IEndpointRouteBuilder app)
    //{
    //    app.MapPost("api/posts", async (Command command, ISender sender) =>
    //    {
    //        Guid postId = await sender.Send(command);

    //        return Results.Ok(postId);
    //    });
    //}
}

public class CreatePostEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("api/posts", async (CreatePostRequest request, ISender sender) =>
        {
            var command = request.Adapt<CreatePost.Command>();

            Guid postId = await sender.Send(command);

            return Results.Ok(postId);
        });
    }
}
