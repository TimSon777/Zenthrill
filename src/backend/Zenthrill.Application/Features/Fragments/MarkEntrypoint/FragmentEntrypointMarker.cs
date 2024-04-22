using Microsoft.EntityFrameworkCore;
using OneOf.Types;
using Zenthrill.Application.Results;
using Zenthrill.Domain.Entities;
using NotFound = Zenthrill.Application.Results.NotFound;

namespace Zenthrill.Application.Features.Fragments.MarkEntrypoint;

public interface IFragmentEntrypointMarker
{
    Task<MarkFragmentEntrypointOneOf> MarkAsync(
        MarkFragmentEntrypointRequest request,
        CancellationToken cancellationToken);
}

public sealed class FragmentEntrypointMarker(
    IApplicationDbContext applicationDbContext,
    IGraphDbContext graphDbContext) : IFragmentEntrypointMarker
{
    public async Task<MarkFragmentEntrypointOneOf> MarkAsync(
        MarkFragmentEntrypointRequest request,
        CancellationToken cancellationToken)
    {
        return await graphDbContext.ExecuteAsync<MarkFragmentEntrypointOneOf>(async (repositoryRegistry, ct) =>
        {
            var storyInfoVersion = await applicationDbContext.StoryInfoVersions
                .Include(siv => siv.StoryInfo)
                .Include(siv => siv.SubVersions)
                .FirstOrDefaultAsync(StoryInfoVersion.ById(request.StoryInfoVersionId), cancellationToken);

            if (storyInfoVersion is null)
            {
                return NotFound.ById(request.StoryInfoVersionId);
            }
            
            if (storyInfoVersion.IsBaseVersion)
            {
                return new ForbidEditBaseVersion();
            }
 
            if (!request.User.HasAccessToUpdate(storyInfoVersion.StoryInfo))
            {
                return new Forbid();
            }

            var fragment = await repositoryRegistry.FragmentRepository
                .TryGetAsync(request.Id, storyInfoVersion.Id, ct);

            if (fragment is null)
            {
                return NotFound.ById(request.Id);
            }

            storyInfoVersion.EntrypointFragmentId = fragment.Id;

            await applicationDbContext.SaveChangesAsync(cancellationToken);

            return new Success();
        }, cancellationToken);
    }
}