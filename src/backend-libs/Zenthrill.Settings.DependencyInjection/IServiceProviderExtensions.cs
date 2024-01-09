using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Zenthrill.Settings.DependencyInjection;

// ReSharper disable once InconsistentNaming
public static class IServiceProviderExtensions
{
    public static T GetOptions<T>(this IServiceProvider serviceProvider)
        where T : class
    {
        return serviceProvider.GetRequiredService<IOptions<T>>().Value;
    }
}