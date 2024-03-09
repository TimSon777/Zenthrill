using Microsoft.EntityFrameworkCore;
using Zenthrill.Application.Results;
using Zenthrill.Application.Specs;

namespace Zenthrill.Application.Features.Stories.Read;

public interface IStoryReader
{
    Task<ReadStoryOneOf> ReadAsync(ReadStoryRequest request, CancellationToken cancellationToken);
}

public sealed class StoryReader(
    IApplicationDbContext applicationDbContext,
    IGraphDbContext graphDbContext) : IStoryReader
{
    public async Task<ReadStoryOneOf> ReadAsync(ReadStoryRequest request,
        CancellationToken cancellationToken)
    {
        return await graphDbContext.ReadAsync<ReadStoryOneOf>(
            async (repositoryRegistry, ct) =>
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
                
                return await repositoryRegistry.StoryRepository
                    .ReadAsync(storyInfo, ct);
            },
            cancellationToken);
    }
}
