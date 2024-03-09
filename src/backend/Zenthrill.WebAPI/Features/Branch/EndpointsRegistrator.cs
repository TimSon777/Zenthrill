using Zenthrill.WebAPI.Common;

namespace Zenthrill.WebAPI.Features.Branch;

public sealed class EndpointsRegistrator : IEndpointsRegistrator
{
    public IEndpointRouteBuilder Register(IEndpointRouteBuilder builder)
    {
        builder
            .MapPost("/branches", Create.Endpoint.Create);

        builder
            .MapPut("/branches", Update.Endpoint.Update);

        return builder;
    }
}