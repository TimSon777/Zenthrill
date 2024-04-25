using Zenthrill.WebAPI.Common;

namespace Zenthrill.WebAPI.Features.Tags;

public sealed class EndpointsRegistrator : IEndpointsRegistrator
{
    public IEndpointRouteBuilder Register(IEndpointRouteBuilder builder)
    {
        builder
            .MapGet("/tags", List.Endpoint.List);
      
        return builder;
    }
}