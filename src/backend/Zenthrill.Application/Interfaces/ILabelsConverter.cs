using Zenthrill.Domain.Entities;

namespace Zenthrill.Application.Interfaces;

public interface ILabelsConverter
{
    string Convert(StoryInfoVersionId storyInfoVersionId);
}