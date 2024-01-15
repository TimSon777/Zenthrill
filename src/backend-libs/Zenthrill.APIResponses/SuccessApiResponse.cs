namespace Zenthrill.APIResponses;

public sealed class SuccessApiResponse : ApiResponse
{
    internal SuccessApiResponse()
        : base(DefaultStatusCodes.Success)
    {
    }
}

public sealed class SuccessApiResponse<T> : ApiResponse
{
    public T Value { get; }

    internal SuccessApiResponse(T value)
        : base(DefaultStatusCodes.Success)
    {
        Value = value;
    }
}