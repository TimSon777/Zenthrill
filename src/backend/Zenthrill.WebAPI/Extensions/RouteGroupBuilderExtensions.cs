using System.Reflection;
using Zenthrill.WebAPI.Common;

namespace Zenthrill.WebAPI.Extensions;

public static class RouteGroupBuilderExtensions
{
    public static IEndpointRouteBuilder UseApi(this IEndpointRouteBuilder builder, Assembly assembly)
    {
        var registrators = assembly
            .GetTypes()
            .Where(t => typeof(IEndpointsRegistrator).IsAssignableFrom(t) && t is { IsInterface: false, IsAbstract: false })
            .Select(Activator.CreateInstance)
            .Cast<IEndpointsRegistrator>();

        foreach (var registrator in registrators)
        {
            registrator.Register(builder);
        }

        return builder;
    }
}