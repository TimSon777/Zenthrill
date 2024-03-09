using Zenthrill.WebAPI.Common;

namespace Zenthrill.WebAPI.Features.Story;

public sealed class EndpointsRegistrator : IEndpointsRegistrator
{
    public IEndpointRouteBuilder Register(IEndpointRouteBuilder builder)
    {
        builder
            .MapGet("/stories", Read.Endpoint.Read);
        
        builder
            .MapPost("/stories", Create.Endpoint.Create);

        builder
            .MapPost("/stories/example", ExampleCreate.Endpoint.CreateExample);

        return builder;
    }
}