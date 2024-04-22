using FluentValidation;

namespace Zenthrill.Application.Features.Fragments.Update;

public sealed class UpdateFragmentRequestValidator : AbstractValidator<UpdateFragmentRequest>
{
    public UpdateFragmentRequestValidator()
    {
        RuleFor(r => r.Body)
            .Length(0, FragmentConstants.BodyMaxLength);
        
        RuleFor(r => r.Name)
            .Length(0, FragmentConstants.NameMaxLength);
    }
}