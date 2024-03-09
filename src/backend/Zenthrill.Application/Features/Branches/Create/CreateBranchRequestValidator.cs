using FluentValidation;

namespace Zenthrill.Application.Features.Branches.Create;

public sealed class CreateBranchRequestValidator : AbstractValidator<CreateBranchRequest>
{
    public CreateBranchRequestValidator()
    {
        RuleFor(r => r.Inscription)
            .Length(1, BranchConstants.InscriptionMaxLength);
    }
}