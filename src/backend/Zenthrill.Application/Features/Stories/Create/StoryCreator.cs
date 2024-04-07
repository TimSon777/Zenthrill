﻿using FluentValidation;
using OneOf;
using Zenthrill.Application.Results;
using Zenthrill.Domain.Entities;
using Zenthrill.Domain.ValueObjects;

namespace Zenthrill.Application.Features.Stories.Create;

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

        var storyInfo = new StoryInfo
        {
            Description = request.Description,
            CreatorId = request.User.Id
        };

        applicationDbContext.StoryInfos.Add(storyInfo);

        await applicationDbContext.SaveChangesAsync(cancellationToken);

        return storyInfo.Id;
    }
}