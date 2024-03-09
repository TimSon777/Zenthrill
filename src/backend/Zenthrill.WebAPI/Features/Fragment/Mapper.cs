using Zenthrill.Application.Features.Fragments;
using Zenthrill.Application.Features.Fragments.Create;
using Zenthrill.Application.Features.Fragments.Update;
using Zenthrill.WebAPI.Features.Fragment.Update;

namespace Zenthrill.WebAPI.Features.Fragment;

public interface IMapper
{
    CreateFragmentRequest MapToApplicationRequest(Create.Request request);

    UpdateFragmentRequest MapToApplicationRequest(Update.Request request);
}

public sealed class Mapper : IMapper
{
    public CreateFragmentRequest MapToApplicationRequest(Create.Request request)
    {
        return new CreateFragmentRequest
        {
            Body = request.Body,
            StoryInfoId = new StoryInfoId(request.StoryInfoId),
            User = new User
            {
                Nickname = "Test",
                Id = new UserId(new Guid("cffc1c0c-3a86-42dc-94c0-5e9a0c6ab5a6"))
            }
        };
    }

    public UpdateFragmentRequest MapToApplicationRequest(Request request)
    {
        return new UpdateFragmentRequest
        {
            StoryInfoId = new StoryInfoId(request.StoryInfoId),
            Body = request.Body,
            FragmentId = new FragmentId(request.FragmentInfoId),
            User = new User
            {
                Nickname = "Test",
                Id = new UserId(new Guid("cffc1c0c-3a86-42dc-94c0-5e9a0c6ab5a6"))
            }
        };
    }
}