using Zenthrill.WebAPI.Common;

namespace Zenthrill.WebAPI.Features.Fragment;

public sealed class EndpointsRegistrator : IEndpointsRegistrator
{
    public IEndpointRouteBuilder Register(IEndpointRouteBuilder builder)
    {
        builder
            .MapPut("/fragments", Update.Endpoint.Update);

        builder
            .MapPost("/fragments", Create.Endpoint.Create);

        return builder;
    }
}