using Zenthrill.WebAPI.Common;
using Endpoint = Zenthrill.WebAPI.Features.Story.ExampleVersionCreate.Endpoint;

namespace Zenthrill.WebAPI.Features.Story;

public sealed class EndpointsRegistrator : IEndpointsRegistrator
{
    public IEndpointRouteBuilder Register(IEndpointRouteBuilder builder)
    {
        builder
            .MapGet("/story-versions/{id}", ReadVersion.Endpoint.ReadVersion)
            .RequireAuthorization();
        
        builder
            .MapPost("/stories", Create.Endpoint.Create)
            .RequireAuthorization();;

        builder
            .MapPost("/story-versions/example", Endpoint.CreateExampleVersion)
            .RequireAuthorization();

        builder
            .MapPost("/story-versions", CreateVersion.Endpoint.CreateVersion)
            .RequireAuthorization();

        builder
            .MapGet("stories/{id}", Read.Endpoint.Read)
            .RequireAuthorization();

        builder
            .MapGet("stories", List.Endpoint.List)
            .RequireAuthorization();

        return builder;
    }
}