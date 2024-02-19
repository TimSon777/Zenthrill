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

namespace Zenthrill.Application.Features.Branch;

public sealed class CreateBranchRequest
{
    public required StoryInfoId StoryInfoId { get; init; }

    public required FragmentId FromFragmentId { get; init; }
    
    public required FragmentId ToFragmentId { get; init; }
    
    public required string Inscription { get; init; }

    public required User User { get; init; }
}

public sealed class CreateBranchRequestValidator : AbstractValidator<CreateBranchRequest>
{
    public CreateBranchRequestValidator()
    {
        RuleFor(r => r.Inscription)
            .Length(1, 1000);
    }
}

public interface IBranchCreator
{
    Task<OneOf<BranchId, ValidationFailure, Forbid, NotFound<StoryInfoId>, FragmentDoesNotExist>> CreateAsync(
        CreateBranchRequest request,
        CancellationToken cancellationToken);
}

public sealed class BranchCreator(
    IApplicationDbContext applicationDbContext,
    IGraphDatabase graphDatabase,
    IOptions<GraphDatabaseSettings> graphDatabaseSettings,
    IValidator<CreateBranchRequest> validator) : IBranchCreator
{
    private readonly GraphDatabaseSettings _graphDatabaseSettings = graphDatabaseSettings.Value;

    public async Task<OneOf<BranchId, ValidationFailure, Forbid, NotFound<StoryInfoId>, FragmentDoesNotExist>> CreateAsync(
        CreateBranchRequest request,
        CancellationToken cancellationToken)
    {
        var storyInfo = await applicationDbContext.StoryInfos
            .Include(storyInfo => storyInfo.Creator)
            .FirstOrDefaultAsync(StoryInfoSpecs.ById(request.StoryInfoId), cancellationToken);

        if (storyInfo is null)
        {
            return NotFound.ById(request.StoryInfoId);
        }

        var hasAccess = request.User.HasAccessToUpdate(storyInfo);

        if (!hasAccess)
        {
            return new Forbid();
        }

        var result = await validator.ValidateAsync(request, cancellationToken);

        if (!result.IsValid)
        {
            return new ValidationFailure(result.ToDictionary());
        }
 
        var from = new FragmentNodeMatchById
        {
            Id = request.FromFragmentId.Value
        };

        var to = new FragmentNodeMatchById
        {
            Id = request.ToFragmentId.Value
        };

        var branchId = BranchId.New();

        var branchRelationship = new BranchRelationship
        {
            Id = branchId.Value,
            Inscription = request.Inscription
        };

        var labels = new[] { StoryLabels.Transition };

        var relationshipsIds = await graphDatabase.CreateRelationshipAsync(_graphDatabaseSettings.DatabaseName, from, to, branchRelationship, labels);

        if (relationshipsIds.Count == 0)
        {
            return new FragmentDoesNotExist();
        }

        return branchId;
    }
}