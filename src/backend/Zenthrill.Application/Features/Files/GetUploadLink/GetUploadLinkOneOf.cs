using OneOf;
using Zenthrill.Application.Results;
using Zenthrill.Domain.Entities;

namespace Zenthrill.Application.Features.Files.GetUploadLink;

public sealed class GetUploadLinkOneOf : OneOfBase<
    Uri,
    ValidationFailure,
    Forbid,
    NotFound<StoryInfoId>>
{
    public GetUploadLinkOneOf(OneOf<Uri, ValidationFailure, Forbid, NotFound<StoryInfoId>> input) : base(input)
    {
    }
    
    public static implicit operator GetUploadLinkOneOf(Uri _) => new(_);
    public static implicit operator GetUploadLinkOneOf(ValidationFailure _) => new(_);
    public static implicit operator GetUploadLinkOneOf(Forbid _) => new(_);
    public static implicit operator GetUploadLinkOneOf(NotFound<StoryInfoId> _) => new(_);
}