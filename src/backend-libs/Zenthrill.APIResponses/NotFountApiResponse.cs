namespace Zenthrill.APIResponses;

public sealed class NotFountApiResponse<TId> : ApiResponse
{
    public TId Id { get; }

    internal NotFountApiResponse(TId id)
        : base(DefaultStatusCodes.NotFound)
    {
        Id = id;
    }
}