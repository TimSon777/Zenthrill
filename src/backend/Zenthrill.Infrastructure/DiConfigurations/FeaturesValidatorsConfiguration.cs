using FluentValidation;
using Zenthrill.Application.Features.Story;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection;

public static class FeaturesValidatorsConfiguration
{
    public static IServiceCollection AddFeaturesValidators(this IServiceCollection services)
    {
        services
            .AddValidatorsFromAssembly(typeof(CreateStoryRequestValidator).Assembly);

        return services;
    }
}