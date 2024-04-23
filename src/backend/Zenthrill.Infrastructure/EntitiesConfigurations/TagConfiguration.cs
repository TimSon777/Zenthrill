using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Zenthrill.Domain.Entities;

namespace Zenthrill.Infrastructure.EntitiesConfigurations;

public sealed class TagConfiguration : IEntityTypeConfiguration<Tag>
{
    public void Configure(EntityTypeBuilder<Tag> builder)
    {
        builder
            .Property(x => x.Id)
            .HasConversion(
                x => x.Value,
                x => new TagId(x));
    }
}