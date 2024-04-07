using FluentValidation;

namespace Zenthrill.Application.Features.Stories.Create;

public sealed class CreateStoryRequestValidator : AbstractValidator<CreateStoryRequest>
{
    public CreateStoryRequestValidator()
    {
        RuleFor(r => r.Description)
            .Length(0, 100);
    }
}