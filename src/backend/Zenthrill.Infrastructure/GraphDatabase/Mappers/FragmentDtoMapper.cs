using Zenthrill.Domain.Entities;
using Zenthrill.Infrastructure.GraphDatabase.Objects;

namespace Zenthrill.Infrastructure.GraphDatabase.Mappers;

public static class FragmentDtoMapper
{
    public static Fragment MapToFragment(this FragmentDto fragmentDto)
    {
        return new Fragment
        {
            Id = new FragmentId(Guid.Parse(fragmentDto.Id)),
            Body = fragmentDto.Body
        };
    }
}