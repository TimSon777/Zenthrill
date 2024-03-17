using FluentValidation;

namespace Zenthrill.Application.Features.Files.GetUploadLink;

public sealed class GetUploadLinkRequestValidator : AbstractValidator<GetUploadLinkRequest>
{
    public GetUploadLinkRequestValidator()
    {
        RuleFor(r => r.Extension)
            .Length(0, 10);
    }
}