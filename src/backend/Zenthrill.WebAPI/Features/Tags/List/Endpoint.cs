using Zenthrill.APIResponses;
using Zenthrill.Application.Features.Tags.List;

namespace Zenthrill.WebAPI.Features.Tags.List;

public static class Endpoint
{
    public static async Task<IResult> List(
        ITagListReader tagListReader,
        IMapper mapper,
        CancellationToken cancellationToken)
    {
        var tags = await tagListReader.ListAsync(cancellationToken);

        var result = mapper.MapFromApplicationResponse(tags);
        
        return TypedResults.Ok(ApiResponses.Success(result));
    }
}