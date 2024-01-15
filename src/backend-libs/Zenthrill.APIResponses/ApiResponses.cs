namespace Zenthrill.APIResponses;

public static class ApiResponses
{
    public static SuccessApiResponse Success()
    {
        return new SuccessApiResponse();
    }

    public static SuccessApiResponse<TValue> Success<TValue>(TValue value)
    {
        return new SuccessApiResponse<TValue>(value);
    }

    public static ErrorApiResponse Failure(
        string code = DefaultStatusCodes.Failure,
        IDictionary<string, string[]>? errors = null)
    {
        return new ErrorApiResponse(code, errors);
    }

    public static NotFountApiResponse<TId> NotFound<TId>(TId id)
    {
        return new NotFountApiResponse<TId>(id);
    }
}