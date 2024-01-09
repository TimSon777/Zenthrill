using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Zenthrill.Domain.Entities;

namespace Zenthrill.Infrastructure.EntitiesConfigurations;

public sealed class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder
            .Property(user => user.Id)
            .HasConversion(
                x => x.Id,
                x => new UserId { Id = x });
    }
}