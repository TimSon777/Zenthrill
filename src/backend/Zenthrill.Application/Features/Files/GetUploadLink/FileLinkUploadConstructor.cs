using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Zenthrill.Application.Interfaces;
using Zenthrill.Application.Results;

namespace Zenthrill.Application.Features.Files.GetUploadLink;

public interface IFileLinkUploadConstructor
{
    Task<GetUploadLinkOneOf> GetUploadLinkAsync(GetUploadLinkRequest request, CancellationToken cancellationToken);
}

public sealed class FileLinkUploadConstructor(
    IApplicationDbContext applicationDbContext,
    IFileService fileService,
    IValidator<GetUploadLinkRequest> validator) : IFileLinkUploadConstructor
{
    public async Task<GetUploadLinkOneOf> GetUploadLinkAsync(GetUploadLinkRequest request, CancellationToken cancellationToken)
    {
        var storyInfo = await applicationDbContext.StoryInfos
            .Include(si => si.Creator)
            .FirstOrDefaultAsync(si => si.Id == request.StoryInfoId, cancellationToken);

        if (storyInfo is null)
        {
            return NotFound.ById(request.StoryInfoId);
        }
        
        if (!request.User.HasAccessToUpdate(storyInfo))
        {
            return new Forbid();
        }

        var result = await validator.ValidateAsync(request, cancellationToken);

        if (!result.IsValid)
        {
            return new ValidationFailure(result.ToDictionary());
        }
        
        var key = $"{storyInfo.Id}/{Guid.NewGuid():N}.{request.Extension}";

        return await fileService.GetLinkForUploadAsync(key, cancellationToken);
    }
}