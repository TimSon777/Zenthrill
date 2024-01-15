using Zenthrill.WebAPI.Common;

namespace Zenthrill.WebAPI.Features.Story;

public sealed class EndpointsRegistrator : IEndpointsRegistrator
{
    public IEndpointRouteBuilder Register(IEndpointRouteBuilder builder)
    {
        builder
            .MapPost("/stories", Create.Endpoint.Create);

        return builder;
    }
}