using FluentValidation;
using OneOf;
using Zenthrill.Application.Results;
using Zenthrill.Domain.Entities;
using Zenthrill.Domain.ValueObjects;

namespace Zenthrill.Application.Features.Story;

public sealed class CreateStoryRequest
{
    public required User User { get; init; }

    public required string Name { get; init; }
}

public sealed class CreateStoryRequestValidator : AbstractValidator<CreateStoryRequest>
{
    public CreateStoryRequestValidator()
    {
        RuleFor(r => r.Name)
            .Length(0, 100);
    }
}

public interface IStoryCreator
{
    Task<OneOf<StoryInfoId, ValidationFailure>> CreateAsync(CreateStoryRequest request, CancellationToken cancellationToken);
}

public sealed class StoryCreator(
    IApplicationDbContext applicationDbContext,
    IValidator<CreateStoryRequest> validator) : IStoryCreator
{
    public async Task<OneOf<StoryInfoId, ValidationFailure>> CreateAsync(CreateStoryRequest request, CancellationToken cancellationToken)
    {
        var result = await validator.ValidateAsync(request, cancellationToken);

        if (!result.IsValid)
        {
            return new ValidationFailure(result.ToDictionary());
        }

        var version = StoryVersion.Create(1, 0, "0").AsT0;

        var entrypointFragmentId = FragmentId.New();

        applicationDbContext.Users.Attach(request.User);
        var storyInfo = new StoryInfo
        {
            Creator = request.User,
            StoryName = request.Name,
            EntrypointFragmentId = entrypointFragmentId,
            Version = version
        };

        applicationDbContext.StoryInfos.Add(storyInfo);

        await applicationDbContext.SaveChangesAsync(cancellationToken);

        return storyInfo.Id;
    }
}