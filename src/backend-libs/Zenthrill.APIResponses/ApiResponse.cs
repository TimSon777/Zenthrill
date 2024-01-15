namespace Zenthrill.APIResponses;

public abstract class ApiResponse(string code)
{
    public string Code { get; } = code;
}