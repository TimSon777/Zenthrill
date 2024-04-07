namespace Zenthrill.Auth.WebAPI.Requests;

public sealed class RegisterRequest
{
    public required string UserName { get; set; }

    public required string Password { get; set; }
}