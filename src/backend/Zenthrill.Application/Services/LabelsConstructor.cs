using Zenthrill.Domain;
using Zenthrill.Domain.Entities;

namespace Zenthrill.Application.Services;

public interface ILabelsConstructor
{
    string ConstructStoryInfoLabel(StoryInfoId storyInfoId);
}

public sealed class LabelsConstructor : ILabelsConstructor
{
    public string ConstructStoryInfoLabel(StoryInfoId storyInfoId)
    {
        return $"{StoryLabels.StoryInfoIdPrefix}{storyInfoId.Value:N}";
    }
}