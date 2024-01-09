using Zenthrill.WebAPI.Common;

namespace Zenthrill.WebAPI.Features.Story;

public sealed class StoryEndpointsRegistrator : IEndpointsRegistrator
{
    public IEndpointRouteBuilder Register(IEndpointRouteBuilder builder)
    {
        builder
            .MapPost("/story", Create.Endpoint.Create);

        return builder;
    }
}