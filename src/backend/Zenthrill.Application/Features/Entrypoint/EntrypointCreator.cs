using Microsoft.Extensions.Options;
using Zenthrill.Application.Services;
using Zenthrill.Application.Settings;
using Zenthrill.Domain;
using Zenthrill.Domain.Entities;

namespace Zenthrill.Application.Features.Entrypoint;

public sealed class CreateEntrypointRequest
{
    public required FragmentId EntrypointFragmentId { get; set; }    
}

public interface IEntrypointCreator
{
    Task CreateAsync(CreateEntrypointRequest request);
}

public sealed class EntrypointCreator(
    IGraphDatabase graphDatabase,
    IOptions<GraphDatabaseSettings> graphDatabaseSettingsOptions) : IEntrypointCreator
{
    public async Task CreateAsync(CreateEntrypointRequest request)
    {
        var databaseName = graphDatabaseSettingsOptions.Value.DatabaseName;
        var entrypointFragment = new EntrypointFragment
        {
            Id = request.EntrypointFragmentId
        };

        var labels = new[] { StoryLabels.EntrypointLabel };
        _ = await graphDatabase.CreateNodeAsync(databaseName, entrypointFragment, labels);
    }
}