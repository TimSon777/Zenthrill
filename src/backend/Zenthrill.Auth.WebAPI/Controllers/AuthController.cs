using System.Security.Claims;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OpenIddict.Abstractions;
using OpenIddict.Server.AspNetCore;
using Zenthrill.APIResponses;
using Zenthrill.Auth.Model.Entities;

namespace Zenthrill.Auth.WebAPI.Controllers;

[ApiController]
[Route("connect")]
public sealed class AuthController(
    UserManager<User> userManager,
    SignInManager<User> signInManager) : ControllerBase
{
    [HttpPost("token")]
    public async Task<IActionResult> Token()
    {
        var request = HttpContext.GetOpenIddictServerRequest()
            ?? throw new InvalidOperationException("Can't retrieve OpenIddictRequest from HttpContext");

        if (!request.IsPasswordGrantType())
        {
            return UnprocessableEntity(ApiResponses.Failure(DefaultStatusCodes.Failure, new Dictionary<string, string[]>
            {
                ["password"] = ["error"]
            }));
        }

        var user = await userManager.FindByNameAsync(request.Username!);

        if (user is null)
        {
            return BadRequest();
        }

        var passwordCheck = await userManager.CheckPasswordAsync(user, request.Password!);

        if (!passwordCheck)
        {
            return BadRequest();
        }

        var principal = await signInManager.CreateUserPrincipalAsync(user);
        var scopes = request.GetScopes().Intersect([
            OpenIddictConstants.Scopes.Email,
            OpenIddictConstants.Scopes.Profile,
            OpenIddictConstants.Scopes.Roles
        ]);

        principal.SetScopes(scopes);

        principal.SetClaim(OpenIddictConstants.Claims.Subject, user.Id.ToString());
        principal.SetClaim(OpenIddictConstants.Claims.Name, user.UserName);

        foreach (var claim in principal.Claims)
        {
            claim.SetDestinations(GetDestinations(principal, claim));
        }

        return SignIn(principal, OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);

    }
    
    private IEnumerable<string> GetDestinations(ClaimsPrincipal principal, Claim claim)
    {
        switch (claim.Type)
        {
            case OpenIddictConstants.Claims.Name:
                yield return OpenIddictConstants.Destinations.AccessToken;
                if (principal.HasScope(OpenIddictConstants.Scopes.Profile))
                {
                    yield return OpenIddictConstants.Destinations.IdentityToken;
                }
                break;

            case OpenIddictConstants.Claims.Role:
                yield return OpenIddictConstants.Destinations.AccessToken;
                if (principal.HasScope(OpenIddictConstants.Scopes.Roles))
                {
                    yield return OpenIddictConstants.Destinations.IdentityToken;
                }
                break;

            case OpenIddictConstants.Claims.Email:
                yield return OpenIddictConstants.Destinations.AccessToken;
                if (principal.HasScope(OpenIddictConstants.Scopes.Email))
                {
                    yield return OpenIddictConstants.Destinations.IdentityToken;
                }
                break;

            case "AspNet.Identity.SecurityStamp":
                // Нет операций, соответствующих "ignore" в F#
                break;

            default:
                yield return OpenIddictConstants.Destinations.AccessToken;
                break;
        }
    }
}