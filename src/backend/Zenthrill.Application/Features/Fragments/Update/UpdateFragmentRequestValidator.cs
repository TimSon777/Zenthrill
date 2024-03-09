using FluentValidation;

namespace Zenthrill.Application.Features.Fragments.Update;

public sealed class UpdateFragmentRequestValidator : AbstractValidator<UpdateFragmentRequest>
{
    public UpdateFragmentRequestValidator()
    {
        RuleFor(r => r.Body)
            .Length(0, FragmentConstants.BodyMaxLength);
    }
}