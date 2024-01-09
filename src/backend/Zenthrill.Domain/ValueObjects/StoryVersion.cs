using OneOf;
using Zenthrill.Domain.Common;
using Zenthrill.Domain.Extensions;

namespace Zenthrill.Domain.ValueObjects;

public sealed record StoryVersion : ValueObject
{
    public int Major { get; private init; }

    public int Minor { get; private set; }

    public string Suffix { get; private set; } = default!;

    private StoryVersion()
    {
    }

    public static OneOf<StoryVersion, MajorNegative, MinorNegative, SuffixEmpty> Create(int major, int minor, string suffix)
    {
        if (major < 0)
        {
            return new MajorNegative();
        }

        if (minor < 0)
        {
            return new MinorNegative();
        }

        if (suffix.IsNullOrWhiteSpace())
        {
            return new SuffixEmpty();
        }

        return new StoryVersion
        {
            Major = major,
            Minor = minor,
            Suffix = suffix.Trim()
        };
    }

    public static StoryVersion FromString(string src)
    {
        var elements = src.Split('.', 3);
        
        var major = int.Parse(elements[0]);
        var minor = int.Parse(elements[1]);
        var suffix = elements[2];

        return new StoryVersion
        {
            Major = major,
            Minor = minor,
            Suffix = suffix
        };
    }
    public override string ToString()
    {
        return $"{Major}.{Minor}.{Suffix}";
    }

    public readonly struct MajorNegative;

    public readonly struct MinorNegative;

    public readonly struct SuffixEmpty;
}
