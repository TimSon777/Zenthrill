namespace Zenthrill.Application.Interfaces;

public interface IFileService
{
    Task<Uri> GetLinkForUploadAsync(string key, CancellationToken cancellationToken);
}