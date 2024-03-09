using FluentValidation;

namespace Zenthrill.Application.Features.Branches.Update;

public sealed class UpdateBranchRequestValidator : AbstractValidator<UpdateBranchRequest>
{
    public UpdateBranchRequestValidator()
    {
        RuleFor(r => r.Inscription)
            .Length(1, BranchConstants.InscriptionMaxLength);
    }
}