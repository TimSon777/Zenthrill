using Zenthrill.WebAPI.Common;

namespace Zenthrill.WebAPI.Features.Files;

public sealed class EndpointsRegistrator : IEndpointsRegistrator
{
    public IEndpointRouteBuilder Register(IEndpointRouteBuilder builder)
    {
        builder
            .MapPut("/files/upload-link", GetUploadLink.Endpoint.GetUploadLink);

        return builder;
    }
}