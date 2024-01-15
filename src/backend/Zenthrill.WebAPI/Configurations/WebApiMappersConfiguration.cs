// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection;

public static class WebApiMappersConfiguration
{
    public static IServiceCollection AddWebApiMappers(this IServiceCollection services)
    {
        return services
            .AddSingleton<Zenthrill.WebAPI.Features.Branch.IMapper, Zenthrill.WebAPI.Features.Branch.Mapper>()
            .AddSingleton<Zenthrill.WebAPI.Features.Fragment.IMapper, Zenthrill.WebAPI.Features.Fragment.Mapper>()
            .AddSingleton<Zenthrill.WebAPI.Features.Story.IMapper, Zenthrill.WebAPI.Features.Story.Mapper>();
    }
}