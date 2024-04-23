using Microsoft.EntityFrameworkCore;
using Zenthrill.Application;

namespace Zenthrill.WebAPI.GraphQL.Schema.Queries;

public sealed class StoryRootQuery
{
    [UsePaging]
    [UseSorting]
    [UseFiltering]
    public IQueryable<StoryInfo> GetStoryInfos(
        [Service] IApplicationDbContext applicationDbContext,
        ICollection<Guid> tagIds)
    {
        IQueryable<StoryInfo> query = applicationDbContext.StoryInfos
            .Include(si => si.Tags);

        return tagIds.Aggregate(query, (current, tagId) =>
            current.Where(si => si.Tags.Any(tag => tag.Id == new TagId(tagId))));
    }
}
