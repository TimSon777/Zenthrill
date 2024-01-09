using FluentValidation;
using OneOf;
using Zenthrill.Application.OutboxMessages;
using Zenthrill.Application.Results;
using Zenthrill.Domain.Entities;
using Zenthrill.Domain.ValueObjects;
using Zenthrill.Outbox.Core;
using Zenthrill.Providers;

namespace Zenthrill.Application.Features.Story;

public sealed class CreateStoryRequest
{
    public required User User { get; set; }

    public required string Name { get; set; }
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
    IDateTimeOffsetProvider dateTimeOffsetProvider,
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

        var entrypointFragmentId = new FragmentId
        {
            Id = Guid.NewGuid()
        };

        applicationDbContext.Users.Attach(request.User);
        var storyInfo = new StoryInfo
        {
            Creator = request.User,
            StoryName = request.Name,
            EntrypointFragmentId = entrypointFragmentId,
            Version = version
        };

        var createStoryEntrypointOutboxMessage = new CreateStoryEntrypointOutboxMessage(entrypointFragmentId);

        var outboxMessage = new OutboxMessage
        {
            ScheduledAt = dateTimeOffsetProvider.UtcNow,
        };

        outboxMessage.SetMessage(createStoryEntrypointOutboxMessage);

        applicationDbContext.StoryInfos.Add(storyInfo);
        applicationDbContext.OutboxMessages.Add(outboxMessage);

        await applicationDbContext.SaveChangesAsync(cancellationToken);

        return storyInfo.Id;
    }
}