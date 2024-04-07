using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Zenthrill.APIResponses;
using Zenthrill.Auth.Model.Entities;
using Zenthrill.Auth.Model.Extensions;
using Zenthrill.Auth.WebAPI.Requests;

namespace Zenthrill.Auth.WebAPI.Controllers;

[ApiController]
[Route("users")]
public sealed class RegisterController(
    UserManager<User> userManager,
    IValidator<RegisterRequest> validator) : ControllerBase
{
    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterRequest request, CancellationToken cancellationToken)
    {
        var validationResult = await validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
        {
            return BadRequest(ApiResponses.Failure(DefaultStatusCodes.BadRequest, validationResult.ToDictionary()));
        }

        var user = new User
        {
            UserName = request.UserName
        };

        var creatingResult = await userManager.CreateAsync(user, request.Password);

        if (!creatingResult.Succeeded)
        {
            return BadRequest(ApiResponses.Failure(DefaultStatusCodes.BadRequest, creatingResult.ToDictionary()));
        }

        return Ok(ApiResponses.Success());
    }
}