using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Zenthrill.Settings.DependencyInjection;

// ReSharper disable once InconsistentNaming
public static class IServiceCollectionExtensions
{
    public static IHostApplicationBuilder ConfigureSettings<TSettings>(this IHostApplicationBuilder builder)
        where TSettings : class, ISettings
    {
        var section = builder.Configuration.GetSection(TSettings.SectionName);
        builder.Services.Configure<TSettings>(section);
        return builder;
    }

    public static TSettings GetSettings<TSettings>(this IHostApplicationBuilder builder)
        where TSettings : class, ISettings, new()
    {
        var settings = new TSettings();
        builder.Configuration.GetSection(TSettings.SectionName).Bind(settings);
        return settings;
    }
}