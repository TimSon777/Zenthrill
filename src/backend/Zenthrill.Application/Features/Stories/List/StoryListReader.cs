using Microsoft.EntityFrameworkCore;
using Zenthrill.Domain.Entities;

namespace Zenthrill.Application.Features.Stories.List;

public interface IStoryListReader
{
    Task<IEnumerable<StoryInfo>> ListAsync(ListStoryRequest request, CancellationToken cancellationToken);
}

public sealed class StoryListReader(IApplicationDbContext applicationDbContext) : IStoryListReader
{
    public async Task<IEnumerable<StoryInfo>> ListAsync(ListStoryRequest request, CancellationToken cancellationToken)
    {
        return await applicationDbContext.StoryInfos
            .Where(si => si.CreatorId == request.User.Id)
            .ToListAsync(cancellationToken);
    }
}