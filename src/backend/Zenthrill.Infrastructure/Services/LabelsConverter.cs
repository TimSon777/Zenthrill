using Zenthrill.Application.Interfaces;
using Zenthrill.Domain.Entities;

namespace Zenthrill.Infrastructure.Services;

public sealed class LabelsConverter : ILabelsConverter
{
    public string Convert(StoryInfoId storyInfoId)
    {
        return $"story{storyInfoId.Value:N}";
    }
}