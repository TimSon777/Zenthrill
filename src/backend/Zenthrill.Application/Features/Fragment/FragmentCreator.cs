using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using OneOf;
using Zenthrill.Application.Objects;
using Zenthrill.Application.Results;
using Zenthrill.Application.Services;
using Zenthrill.Application.Settings;
using Zenthrill.Application.Specs;
using Zenthrill.Domain;
using Zenthrill.Domain.Entities;

namespace Zenthrill.Application.Features.Fragment;

public sealed class CreateFragmentRequest
{
    public required StoryInfoId StoryInfoId { get; init; }
    
    public required string Body { get; init; }
    
    public required User User { get; init; }
}

public sealed class CreateFragmentRequestValidator : AbstractValidator<CreateFragmentRequest>
{
    public CreateFragmentRequestValidator()
    {
        RuleFor(r => r.Body)
            .Length(0, 20000);
    }
}

public interface IFragmentCreator
{
    Task<OneOf<FragmentId, ValidationFailure, Forbid, NotFound<StoryInfoId>>> CreateAsync(CreateFragmentRequest request, CancellationToken cancellationToken);
}

public sealed class FragmentCreator(
    IApplicationDbContext applicationDbContext,
    IGraphDatabase graphDatabase,
    IValidator<CreateFragmentRequest> validator,
    IOptions<GraphDatabaseSettings> graphDatabaseSettings) : IFragmentCreator
{
    private readonly GraphDatabaseSettings _graphDatabaseSettings = graphDatabaseSettings.Value;

    public async Task<OneOf<FragmentId, ValidationFailure, Forbid, NotFound<StoryInfoId>>> CreateAsync(CreateFragmentRequest request, CancellationToken cancellationToken)
    {
        var storyInfo = await applicationDbContext.StoryInfos
            .Include(storyInfo => storyInfo.Creator)
            .FirstOrDefaultAsync(StoryInfoSpecs.ById(request.StoryInfoId), cancellationToken);

        if (storyInfo is null)
        {
            return NotFound.ById(request.StoryInfoId);
        }

        if (!request.User.HasAccessToUpdate(storyInfo))
        {
            return new Forbid();
        }

        var result = await validator.ValidateAsync(request, cancellationToken);

        if (!result.IsValid)
        {
            return new ValidationFailure(result.ToDictionary());
        }

        var fragmentId = FragmentId.New();

        var node = new FragmentNode
        {
            Id = fragmentId.Value,
            Body = request.Body
        };

        var storyInfoIdLabel = $"{StoryLabels.StoryInfoIdPrefix}{request.StoryInfoId.Value:N}";
        
        var labels = new [] { storyInfoIdLabel, StoryLabels.Fragment };

        await graphDatabase.CreateNodeAsync(_graphDatabaseSettings.DatabaseName, node, labels);

        return fragmentId;
    }
}