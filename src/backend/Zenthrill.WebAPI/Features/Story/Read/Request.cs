using Microsoft.AspNetCore.Mvc;

namespace Zenthrill.WebAPI.Features.Story.Read;

public sealed class Request
{
    [FromRoute(Name = "id")]
    public required Guid Id { get; set; }
}