using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Zenthrill.APIResponses;
using Zenthrill.Auth.Model.Entities;
using Zenthrill.Auth.WebAPI.Responses;

namespace Zenthrill.Auth.WebAPI.Controllers;

[ApiController]
[Route("users")]
public sealed class UsersController(UserManager<User> userManager) : ControllerBase
{
    [HttpGet("{userId}")]
    public async Task<IActionResult> GetUser(Guid userId, CancellationToken cancellationToken)
    {
        var user = await userManager.FindByIdAsync(userId.ToString());

        if (user is null)
        {
            return UnprocessableEntity(ApiResponses.NotFound(userId));
        }

        var roles = await userManager.GetRolesAsync(user);

        var response = new UserResponse
        {
            Id = user.Id,
            UserName = user.UserName!,
            Roles = roles
        };

        return Ok(ApiResponses.Success(response));
    }
}