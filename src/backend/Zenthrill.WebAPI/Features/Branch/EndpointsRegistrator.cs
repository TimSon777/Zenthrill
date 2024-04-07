using Zenthrill.WebAPI.Common;

namespace Zenthrill.WebAPI.Features.Branch;

public sealed class EndpointsRegistrator : IEndpointsRegistrator
{
    public IEndpointRouteBuilder Register(IEndpointRouteBuilder builder)
    {
        builder
            .MapPost("/branches", Create.Endpoint.Create)
            .RequireAuthorization();

        builder
            .MapPut("/branches", Update.Endpoint.Update)
            .RequireAuthorization();

        return builder;
    }
}