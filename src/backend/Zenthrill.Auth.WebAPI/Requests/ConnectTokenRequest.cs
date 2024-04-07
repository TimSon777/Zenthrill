namespace Zenthrill.Auth.WebAPI.Requests;

public sealed class ConnectTokenRequest
{
    public required string UserName { get; set; }
    
    public required string Password { get; set; }
    
    public required string GrantType { get; set; }
}