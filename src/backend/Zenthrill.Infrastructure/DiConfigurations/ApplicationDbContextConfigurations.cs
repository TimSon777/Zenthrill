using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Zenthrill.Application;
using Zenthrill.Application.Settings;
using Zenthrill.Infrastructure;
using Zenthrill.Settings.DependencyInjection;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection;

public static class ApplicationDbContextConfigurations
{
    public static IHostApplicationBuilder AddApplicationDbContextConfiguration(this IHostApplicationBuilder builder)
    {
        builder.ConfigureSettings<MainDatabaseSettings>();

        builder.Services.AddDbContext<IApplicationDbContext, ApplicationDbContext>((sp, options) =>
        {
            var settings = sp.GetOptions<MainDatabaseSettings>();
            options.UseNpgsql(settings.ConnectionString);
        });

        return builder;
    }
}