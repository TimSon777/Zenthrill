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
                x => x.Value,
                x => new StoryInfoId(x));

        builder
            .Property(x => x.CreatorId)
            .HasConversion(
                x => x.Value,
                x => new UserId(x));
        
        builder
            .HasMany(s => s.Tags)
            .WithMany(t => t.StoryInfos)
            .UsingEntity<Dictionary<string, object>>(
                "StoryInfosTags",
                j => j.HasOne<Tag>().WithMany().HasForeignKey("TagId"),
                j => j.HasOne<StoryInfo>().WithMany().HasForeignKey("StoryInfoId"),
                j =>
                {
                    j.HasKey("StoryInfoId", "TagId");
                });
    }
}