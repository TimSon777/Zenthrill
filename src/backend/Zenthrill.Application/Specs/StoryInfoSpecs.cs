using Zenthrill.Domain.Entities;
using Zenthrill.Specifications;

namespace Zenthrill.Application.Specs;

public static class StoryInfoSpecs
{
    public static Specification<StoryInfo> ById(StoryInfoId storyInfoId)
    {
        return new Specification<StoryInfo>(storyInfo => storyInfo.Id == storyInfoId);
    }
}