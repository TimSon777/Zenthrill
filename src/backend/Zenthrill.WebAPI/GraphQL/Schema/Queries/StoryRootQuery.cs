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
        IEnumerable<Guid> tagIds)
    {
        IQueryable<StoryInfo> query = applicationDbContext.StoryInfos
            .Include(si => si.Tags)
            .Include(si => si.Versions
                .Where(siv => siv.EntrypointFragmentId != null && siv.IsPublished));

        return tagIds.Aggregate(query, (current, tagId) =>
            current.Where(si => si.Tags.Any(tag => tag.Id == new TagId(tagId))));
    }
}
