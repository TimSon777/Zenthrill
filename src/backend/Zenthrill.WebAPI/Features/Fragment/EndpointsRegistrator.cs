using Zenthrill.WebAPI.Common;

namespace Zenthrill.WebAPI.Features.Fragment;

public sealed class EndpointsRegistrator : IEndpointsRegistrator
{
    public IEndpointRouteBuilder Register(IEndpointRouteBuilder builder)
    {
        builder
            .MapPut("/fragments", Update.Endpoint.Update)
            .RequireAuthorization();

        builder
            .MapPost("/fragments", Create.Endpoint.Create)
            .RequireAuthorization();

        return builder;
    }
}