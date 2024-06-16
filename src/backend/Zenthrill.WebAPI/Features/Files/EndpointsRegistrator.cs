using Zenthrill.WebAPI.Common;

namespace Zenthrill.WebAPI.Features.Files;

public sealed class EndpointsRegistrator : IEndpointsRegistrator
{
    public IEndpointRouteBuilder Register(IEndpointRouteBuilder builder)
    {
        builder
            .MapGet("/files/upload-link", GetUploadLink.Endpoint.GetUploadLink)
            .RequireAuthorization();

        return builder;
    }
}