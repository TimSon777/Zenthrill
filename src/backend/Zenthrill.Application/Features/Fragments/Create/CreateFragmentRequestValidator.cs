using FluentValidation;

namespace Zenthrill.Application.Features.Fragments.Create;

public sealed class CreateFragmentRequestValidator : AbstractValidator<CreateFragmentRequest>
{
    public CreateFragmentRequestValidator()
    {
        RuleFor(r => r.Body)
            .Length(0, FragmentConstants.BodyMaxLength);
        
        RuleFor(r => r.Name)
            .Length(0, FragmentConstants.NameMaxLength);
    }
}