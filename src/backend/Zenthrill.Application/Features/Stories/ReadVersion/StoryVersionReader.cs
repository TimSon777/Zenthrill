using Microsoft.EntityFrameworkCore;
using Zenthrill.Application.Results;
using Zenthrill.Domain.Entities;

namespace Zenthrill.Application.Features.Stories.ReadVersion;

public interface IStoryVersionReader
{
    Task<ReadStoryVersionOneOf> ReadAsync(ReadStoryVersionRequest versionRequest, CancellationToken cancellationToken);
}

public sealed class StoryVersionVersionReader(
    IApplicationDbContext applicationDbContext,
    IGraphDbContext graphDbContext) : IStoryVersionReader
{
    public async Task<ReadStoryVersionOneOf> ReadAsync(ReadStoryVersionRequest versionRequest,
        CancellationToken cancellationToken)
    {
        return await graphDbContext.ReadAsync<ReadStoryVersionOneOf>(
            async (repositoryRegistry, ct) =>
            {
                var storyInfoVersion = await applicationDbContext.StoryInfoVersions
                    .Include(siv => siv.StoryInfo)
                    .FirstOrDefaultAsync(StoryInfoVersion.ById(versionRequest.StoryInfoVersionId), cancellationToken);

                if (storyInfoVersion is null)
                {
                    return NotFound.ById(versionRequest.StoryInfoVersionId);
                }

                if (!versionRequest.User.HasAccessToRead(storyInfoVersion.StoryInfo))
                {
                    return new Forbid();
                }
                
                return await repositoryRegistry.StoryRepository
                    .ReadAsync(storyInfoVersion, ct);
            },
            cancellationToken);
    }
}
