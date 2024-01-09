using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Zenthrill.Domain.Entities;
using Zenthrill.Domain.ValueObjects;

namespace Zenthrill.Infrastructure.EntitiesConfigurations;

public sealed class StoryInfoConfiguration : IEntityTypeConfiguration<StoryInfo>
{
    public void Configure(EntityTypeBuilder<StoryInfo> builder)
    {
        builder
            .Property(storyInfo => storyInfo.Id)
            .HasConversion(
                x => x.Id,
                x => new StoryInfoId { Id = x });

        builder
            .Property(storyInfo => storyInfo.Version)
            .HasConversion(
                x => x.ToString(),
                x => StoryVersion.FromString(x));

        builder
            .Property(storyInfo => storyInfo.EntrypointFragmentId)
            .HasConversion(
                x => x.Id,
                x => new FragmentId { Id = x });
        
        builder
            .Property(storyInfo => storyInfo.CreatorId)
            .HasConversion(
                x => x.Id,
                x => new UserId { Id = x });
    }
}