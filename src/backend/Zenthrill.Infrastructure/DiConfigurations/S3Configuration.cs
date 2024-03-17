using Amazon.S3;
using Microsoft.Extensions.Hosting;
using Zenthrill.Application.Interfaces;
using Zenthrill.Infrastructure.Services;
using Zenthrill.Infrastructure.Settings;
using Zenthrill.Settings.DependencyInjection;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection;

public static class S3Configuration
{
    public static IHostApplicationBuilder AddS3Configuration(this IHostApplicationBuilder builder)
    {
        builder.ConfigureSettings<S3Settings>();

        builder.Services.AddSingleton<IAmazonS3>(sp =>
        {
            var settings = sp.GetOptions<S3Settings>();

            var config = new AmazonS3Config
            {
                ServiceURL = settings.AwsEndpointUrlS3,
                UseHttp = true,
                ForcePathStyle = true
            };

            return new AmazonS3Client(settings.AwsAccessKeyId, settings.AwsSecretAccessKey, config);
        });

        builder.Services.AddSingleton<IFileService, FileService>();

        return builder;
    }
}