using Microsoft.Extensions.Configuration;
using Zenthrill.Settings;

namespace Zenthrill.Infrastructure.Settings;

public sealed class S3Settings : ISettings
{
    public static string SectionName => "S3";
    
    [ConfigurationKeyName("AWS_ACCESS_KEY_ID")]
    public required string AwsAccessKeyId { get; set; }
    
    [ConfigurationKeyName("AWS_SECRET_ACCESS_KEY")]
    public required string AwsSecretAccessKey { get; set; }
    
    [ConfigurationKeyName("AWS_ENDPOINT_URL_S3")]
    public required string AwsEndpointUrlS3 { get; set; }

    [ConfigurationKeyName("BUCKET_NAME")]
    public required string BucketName { get; set; }
}