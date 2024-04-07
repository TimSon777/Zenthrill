using Zenthrill.Application.Interfaces;
using Zenthrill.Domain.Entities;

namespace Zenthrill.Infrastructure.Services;

public sealed class LabelsConverter : ILabelsConverter
{
    public string Convert(StoryInfoVersionId storyInfoVersionId)
    {
        return $"story{storyInfoVersionId.Value:N}";
    }
}