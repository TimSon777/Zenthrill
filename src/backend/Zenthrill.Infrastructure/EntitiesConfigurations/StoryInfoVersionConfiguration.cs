using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Zenthrill.Domain.Entities;
using Zenthrill.Domain.ValueObjects;

namespace Zenthrill.Infrastructure.EntitiesConfigurations;

public sealed class StoryInfoVersionConfiguration : IEntityTypeConfiguration<StoryInfoVersion>
{
    public void Configure(EntityTypeBuilder<StoryInfoVersion> builder)
    {
        builder
            .Property(storyInfoVersion => storyInfoVersion.Id)
            .HasConversion(
                x => x.Value,
                x => new StoryInfoVersionId(x));
 
        builder
            .Property(storyInfo => storyInfo.Version)
            .HasConversion(
                x => x.ToString(),
                x => StoryVersion.FromString(x));

        builder
            .Property(storyInfo => storyInfo.BaseVersionId)
            .HasConversion(
                x => x == null ? null as Guid? : x.Value.Value,
                x => x == null ? null : new StoryInfoVersionId(x.Value));
            
        builder
            .Property(storyInfo => storyInfo.EntrypointFragmentId)
            .HasConversion(
                x => x == null ? null as Guid? : x.Value.Value,
                x => x == null ? null : new FragmentId(x.Value));
    }
}