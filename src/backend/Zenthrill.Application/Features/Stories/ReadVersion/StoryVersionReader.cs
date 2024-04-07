using Microsoft.EntityFrameworkCore;
using Zenthrill.Application.Features.Stories.ReadVersion;
using Zenthrill.Application.Results;
using Zenthrill.Application.Specs;
using Zenthrill.Domain.Entities;

namespace Zenthrill.Application.Features.Stories.Read;

public interface IStoryReader
{
    Task<ReadStoryVersionOneOf> ReadAsync(ReadStoryVersionRequest versionRequest, CancellationToken cancellationToken);
}

public sealed class StoryVersionReader(
    IApplicationDbContext applicationDbContext,
    IGraphDbContext graphDbContext) : IStoryReader
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
