using Amazon.S3;
using Amazon.S3.Model;
using Microsoft.Extensions.Options;
using Zenthrill.Application.Interfaces;
using Zenthrill.Infrastructure.Settings;

namespace Zenthrill.Infrastructure.Services;

public sealed class FileService(
    IAmazonS3 amazonS3,
    IOptions<S3Settings> settings) : IFileService
{
    private readonly S3Settings _settings = settings.Value;

    public async Task<Uri> GetLinkForUploadAsync(string key, CancellationToken cancellationToken)
    {
        var request = new GetPreSignedUrlRequest
        {
            BucketName = _settings.BucketName,
            Key = key,
            Expires = DateTime.UtcNow.AddHours(1),
            Verb = HttpVerb.PUT,
            Protocol = Protocol.HTTP
        };

        var link = await amazonS3.GetPreSignedURLAsync(request);

        return new Uri(link);
    }
}