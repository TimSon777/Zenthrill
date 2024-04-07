using FluentMigrator.Runner;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Zenthrill.Migrations.Migrations.Main;

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

var serviceProvider = new ServiceCollection()
    .AddFluentMigratorCore()
    .ConfigureRunner(rb => rb
        .AddPostgres()
        .WithGlobalConnectionString(connectionString)
        .ScanIn(typeof(AddStoryTables).Assembly).For.Migrations())
    .AddLogging(lb => lb.AddFluentMigratorConsole())
    .BuildServiceProvider();

var migrationRunner = serviceProvider.GetRequiredService<IMigrationRunner>();

migrationRunner.MigrateUp();
