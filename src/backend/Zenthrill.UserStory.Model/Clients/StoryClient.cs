using System.Net.Http.Json;
using Zenthrill.UserStory.Model.Clients.Dto;

namespace Zenthrill.UserStory.Model.Clients;

public interface IStoryClient
{
    Task<StoryVersionResponse> GetStoryVersionAsync(Guid storyInfoVersionId, CancellationToken cancellationToken);
    
    Task<FragmentResponse> GetFragmentAsync(Guid storyInfoVersionId, Guid fragmentId, CancellationToken cancellationToken);
}

public sealed class StoryClient(IHttpClientFactory httpClientFactory) : IStoryClient
{
    public async Task<StoryVersionResponse> GetStoryVersionAsync(Guid storyInfoVersionId, CancellationToken cancellationToken)
    {
        var httpClient = httpClientFactory.CreateClient("Story");

        var response = await httpClient.GetFromJsonAsync<StoryVersionResponse>(
            $"story-versions/{storyInfoVersionId}",
            cancellationToken);

        return response!;
    }

    public async Task<FragmentResponse> GetFragmentAsync(Guid storyInfoVersionId, Guid fragmentId, CancellationToken cancellationToken)
    {
        var httpClient = httpClientFactory.CreateClient("Story");

        Console.WriteLine($"story-versions/{storyInfoVersionId}/fragments/{fragmentId}");
        var response = await httpClient.GetFromJsonAsync<FragmentResponse>(
            $"story-versions/{storyInfoVersionId}/fragments/{fragmentId}",
            cancellationToken);

        return response!;
    }
}

