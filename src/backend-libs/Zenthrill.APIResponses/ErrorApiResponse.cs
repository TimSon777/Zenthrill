namespace Zenthrill.APIResponses;

public sealed class ErrorApiResponse : ApiResponse
{
    public IDictionary<string, string[]>? Errors { get; }

    internal ErrorApiResponse(string code, IDictionary<string, string[]>? errors)
        : base(code)
    {
        Errors = errors;
    }
}