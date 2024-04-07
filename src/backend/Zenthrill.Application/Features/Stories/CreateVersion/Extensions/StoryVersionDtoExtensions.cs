using Zenthrill.Application.Features.Stories.CreateVersion.Objects;
using Zenthrill.Domain.ValueObjects;

namespace Zenthrill.Application.Features.Stories.CreateVersion.Extensions;

public static class StoryVersionDtoExtensions
{
    public static bool Equals(this StoryVersionDto versionDto, StoryVersion version)
    {
        return versionDto.Major == version.Major
            && versionDto.Minor == version.Minor
            && versionDto.Suffix == version.Suffix;

    }
}