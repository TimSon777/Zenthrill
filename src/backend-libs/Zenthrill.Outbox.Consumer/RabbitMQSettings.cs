using Microsoft.Extensions.Configuration;

namespace Zenthrill.Outbox.Consumer;

public sealed class RabbitMQSettings
{
    [ConfigurationKeyName("HOST")]
    public required string Host { get; set; }

    [ConfigurationKeyName("PASSWORD")]
    public required string Password { get; set; }
    
    [ConfigurationKeyName("USERNAME")]
    public required string Username { get; set; }
    
    [ConfigurationKeyName("PORT")]
    public required int Port { get; set; }
    
    [ConfigurationKeyName("QUEUE")]
    public required string Queue { get; set; }
}