using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Zenthrill.Domain.Entities;

namespace Zenthrill.Application.Features.Stories.CreateVersion;

public sealed class CreateStoryVersionRequestValidator : AbstractValidator<CreateStoryVersionRequest>
{
    public CreateStoryVersionRequestValidator(IApplicationDbContext applicationDbContext)
    {
        RuleFor(request => request.Name)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .Length(0, 100);

        RuleFor(request => request.Version)
            .CustomAsync(async (version, context, ct) =>
            {
                if (version.Major <= 0)
                {
                    context.AddFailure("Version.Major must be greater then 0.");
                }

                if (version.Minor < 0)
                {
                    context.AddFailure("Version.Minor must be greater or equal 0.");
                }

                if (string.IsNullOrWhiteSpace(version.Suffix))
                {
                    context.AddFailure("Version.Suffix can't be null or empty.");
                    return;
                }

                if (!context.RootContextData.TryGetValue("BaseVersion", out var value))
                {
                    return;
                }
                
                if (value is not StoryInfoVersion storyInfoVersion)
                {
                    return;
                }

                Console.WriteLine();
                var sameVersionExists = await applicationDbContext.StoryInfoVersions
                    .AnyAsync(siv =>
                            siv.StoryInfoId == storyInfoVersion.StoryInfoId
                            && siv.Version.Major == version.Major
                            && siv.Version.Minor == version.Minor
                            && siv.Version.Suffix == version.Suffix,
                        ct);

                if (sameVersionExists)
                {
                    context.AddFailure("Same version already exists.");
                    return;
                }

                if (storyInfoVersion.Version.Major < version.Major)
                {
                    context.AddFailure("BaseVersion.Version.Major must be greater or equal then Version.Major");
                }

                if (storyInfoVersion.Version.Major == version.Major && storyInfoVersion.Version.Minor < version.Minor)
                {
                    context.AddFailure("BaseVersion.Minor must be greater or equal then Version.");
                }
            });
    }
}