namespace Zenthrill.WebAPI.Common;

public interface IEndpointsRegistrator
{
    IEndpointRouteBuilder Register(IEndpointRouteBuilder builder);
}