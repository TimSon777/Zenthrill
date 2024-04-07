using Microsoft.AspNetCore.Identity;

namespace Zenthrill.Auth.Model.Extensions;

public static class IdentityResultExtensions
{
    public static Dictionary<string, string[]> ToDictionary(this IdentityResult identityResult)
    {
        return identityResult.Errors
            .GroupBy(e => e.Code)
            .ToDictionary(
                grouping => grouping.Key,
                grouping => grouping.Select(e => e.Description).ToArray());
    }
}