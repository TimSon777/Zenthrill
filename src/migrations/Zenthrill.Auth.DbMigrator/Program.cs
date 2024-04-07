using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Zenthrill.Auth.Model.Entities;
using Zenthrill.Auth.Model.Infrastructure.EntityFrameworkCore;

var currentDirectory = Directory.GetCurrentDirectory();

var environmentName = Environment.GetEnvironmentVariable("DOTNET_ENVIRONMENT")
    ?? throw new InvalidOperationException("\"DOTNET_ENVIRONMENT\" doesn't set");

var configuration = new ConfigurationBuilder()
    .SetBasePath(currentDirectory)
    .AddJsonFile("appsettings.json", true)
    .AddJsonFile($"appsettings.{environmentName}.json", true)
    .AddEnvironmentVariables()
    .Build();

var connectionString = configuration["DATABASE_CONNECTION_STRING"]
                       ?? throw new InvalidOperationException("DATABASE_CONNECTION_STRING doesn't set");

var services = new ServiceCollection();
services
    .AddDbContext<ApplicationDbContext>(options =>
    {
        options.UseNpgsql(connectionString);
    })
    .AddIdentityCore<User>()
    .AddRoles<Role>()
    .AddEntityFrameworkStores<ApplicationDbContext>();

var serviceProvider = services.BuildServiceProvider();

using var scope = serviceProvider.CreateScope();
var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

await context.Database.MigrateAsync();

var roleNames = new[] { "Admin", "User" };

var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<Role>>();

foreach (var roleName in roleNames)
{
    if (!await roleManager.RoleExistsAsync(roleName))
    {
        var role = new Role
        {
            Name = roleName
        };

        var result = await roleManager.CreateAsync(role);
        if (!result.Succeeded)
        {
            foreach (var error in result.Errors)
            {
                Console.WriteLine("Failed to create role with message '{0}'", error.Description);
            }
        }
        else
        {
            Console.WriteLine("Creating role {0}", roleName);
        }
    }
    else
    {
        Console.WriteLine("Role with name {0} already exists", roleName);
    }
}