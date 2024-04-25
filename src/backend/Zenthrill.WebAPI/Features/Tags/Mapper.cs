using Zenthrill.WebAPI.Objects;
using Tag = Zenthrill.Domain.Entities.Tag;

namespace Zenthrill.WebAPI.Features.Tags;

public interface IMapper
{
    List.Response MapFromApplicationResponse(IEnumerable<Tag> tags);
}

public sealed class Mapper : IMapper
{
    public List.Response MapFromApplicationResponse(IEnumerable<Tag> tags)
    {
        return new List.Response
        {
            Tags = tags.Select(t => new TagDto
            {
                Id = t.Id.Value,
                Name = t.Name
            })
        };
    }
}