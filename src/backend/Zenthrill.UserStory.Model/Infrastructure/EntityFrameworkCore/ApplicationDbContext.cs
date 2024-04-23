using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Zenthrill.UserStory.Model.Entities;
using Zenthrill.UserStory.Model.Models;

namespace Zenthrill.UserStory.Model.Infrastructure.EntityFrameworkCore;

public sealed class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
{
    public DbSet<User> Users => Set<User>();

    public DbSet<Story> Stories => Set<Story>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Story>()
            .Property(s => s.ExecutionContext)
            .HasConversion<string>(
                x => JsonSerializer.Serialize(x, JsonSerializerOptions.Default),
                x => JsonSerializer.Deserialize<StoryExecutionContext>(x, JsonSerializerOptions.Default)!);
    }
}