using Microsoft.EntityFrameworkCore;
using Zenthrill.Application.Results;
using Zenthrill.Domain.Aggregates;
using Zenthrill.Domain.Entities;

namespace Zenthrill.Application.Features.Stories.Read;

public interface IStoryReader
{
    Task<ReadStoryOneOf> ReadAsync(ReadStoryRequest request, CancellationToken cancellationToken);
}

public sealed class StoryReader(IApplicationDbContext applicationDbContext) : IStoryReader
{
    public async Task<ReadStoryOneOf> ReadAsync(ReadStoryRequest request, CancellationToken cancellationToken)
    {
        var storyInfo = await applicationDbContext.StoryInfos
            .Include(si => si.Versions)
            .Include(si => si.Tags)
            .FirstOrDefaultAsync(StoryInfo.ById(request.StoryInfoId), cancellationToken);

        if (storyInfo is null)
        {
            return NotFound.ById(request.StoryInfoId);
        }

        if (!request.User.HasAccessToRead(storyInfo))
        {
            return new Forbid();
        }

        return new Story
        {
            Description = storyInfo.Description,
            Versions = storyInfo.Versions.ToList(),
            StoryInfoId = storyInfo.Id,
            Tags = storyInfo.Tags
        };
    }
}