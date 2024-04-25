using Microsoft.EntityFrameworkCore;
using Zenthrill.Domain.Entities;

namespace Zenthrill.Application.Features.Tags.List;

public interface ITagListReader
{
    Task<IEnumerable<Tag>> ListAsync(CancellationToken cancellationToken);
}

public sealed class TagListReader(IApplicationDbContext applicationDbContext) : ITagListReader
{
    public async Task<IEnumerable<Tag>> ListAsync(CancellationToken cancellationToken)
    {
        return await applicationDbContext.Tags
            .ToListAsync(cancellationToken);
    }
}