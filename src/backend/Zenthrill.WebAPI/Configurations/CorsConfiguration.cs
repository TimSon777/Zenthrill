using Zenthrill.Settings.DependencyInjection;
using Zenthrill.WebAPI.Settings;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection;

public static class CorsConfiguration
{
    public static IHostApplicationBuilder AddCors(this IHostApplicationBuilder builder)
    {
        builder.ConfigureSettings<CorsSettings>();
        var settings = builder.GetSettings<CorsSettings>();

        builder.Services.AddCors(options =>
        {
            options.AddPolicy("Frontend",
                b => b
                    .WithOrigins(settings.Origins.ToArray())
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials());
        });

        return builder;
    }
}