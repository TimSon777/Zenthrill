using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using OneOf;
using Zenthrill.Application.Extensions;
using Zenthrill.Application.Results;
using Zenthrill.Application.Services;
using Zenthrill.Application.Settings;
using Zenthrill.Application.Specs;
using Zenthrill.Domain;
using Zenthrill.Domain.Entities;

namespace Zenthrill.Application.Features.Story;

public sealed class ReadStoryFragment
{
    public required FragmentId Id { get; set; }

    public required string Body { get; set; }

    public required IEnumerable<string> Labels { get; set; }
}

public sealed class ReadStoryBranch
{
    public required BranchId Id { get; set; }

    public required string Inscription { get; set; }
    
    public required FragmentId FromFragmentId { get; set; }

    public required FragmentId ToFragmentId { get; set; }

    public required string Type { get; set; }
}

public sealed class ReadStoryResponse
{
    public required ReadStoryFragment? Entrypoint { get; set; }
    
    public required List<ReadStoryFragment> Fragments { get; set; }
    
    public required List<ReadStoryBranch> Branches { get; set; }
}

public sealed class ReadStoryRequest
{
    public required StoryInfoId StoryInfoId { get; set; }

    public required User User { get; set; }
}

public interface IStoryReader
{
    Task<OneOf<ReadStoryResponse, NotFound<StoryInfoId>, Forbid>> ReadAsync(ReadStoryRequest request, CancellationToken cancellationToken);
}

public sealed class StoryReader(
    IApplicationDbContext applicationDbContext,
    IOptions<GraphDatabaseSettings> graphDatabaseSettings,
    IGraphDatabase graphDatabase,
    ILabelsConstructor labelsConstructor) : IStoryReader
{
    private readonly GraphDatabaseSettings _graphDatabaseSettings = graphDatabaseSettings.Value;

    public async Task<OneOf<ReadStoryResponse, NotFound<StoryInfoId>, Forbid>> ReadAsync(ReadStoryRequest request,
        CancellationToken cancellationToken)
    {
        var storyInfo = await applicationDbContext.StoryInfos
            .Include(storyInfo => storyInfo.Creator)
            .FirstOrDefaultAsync(StoryInfoSpecs.ById(request.StoryInfoId), cancellationToken);

        if (storyInfo is null)
        {
            return NotFound.ById(request.StoryInfoId);
        }

        if (!request.User.HasAccessToRead(storyInfo))
        {
            return new Forbid();
        }

        var label = labelsConstructor.ConstructStoryInfoLabel(request.StoryInfoId);
        var result =
            await graphDatabase.GetNodesAndRelationshipsAsync<Node, Relationship>(
                _graphDatabaseSettings.DatabaseName, label);

        var (entrypoint, nodes) = result.Nodes
            .Select(x => new ReadStoryFragment
            {
                Id = new FragmentId(x.Id),
                Body = x.Body,
                Labels = x.Labels
            })
            .DivideBy(n => n.Labels.Contains(StoryLabels.Entrypoint));

        var relationships = result.Relationships
            .Select(r => new ReadStoryBranch
            {
                Id = new BranchId(r.Id),
                FromFragmentId = new FragmentId(r.FromId),
                Inscription = r.Inscription,
                ToFragmentId = new FragmentId(r.ToId),
                Type = r.Type
            });

        return new ReadStoryResponse
        {
            Fragments = nodes.ToList(),
            Branches = relationships.ToList(),
            Entrypoint = entrypoint.FirstOrDefault()
        };
    }
}

file sealed class Node
{
    public required Guid Id { get; set; }

    public required string Body { get; set; }

    public required IEnumerable<string> Labels { get; set; }
}

file sealed class Relationship
{
    public required Guid Id { get; set; }

    public required string Inscription { get; set; }
    
    public required Guid FromId { get; set; }

    public required Guid ToId { get; set; }

    public required string Type { get; set; }
}
