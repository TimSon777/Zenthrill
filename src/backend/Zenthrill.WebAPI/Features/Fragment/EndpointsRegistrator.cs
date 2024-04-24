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

        builder
            .MapPut("/fragments/entrypoint", MarkEntrypoint.Endpoint.Mark)
            .RequireAuthorization();

        builder
            .MapGet("/story-versions/{storyInfoVersionId}/fragments/{fragmentId}", Read.Endpoint.Read);
        
        return builder;
    }
}