using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Zenthrill.Auth.Model.Entities;
using Zenthrill.Auth.WebAPI.Requests;

namespace Zenthrill.Auth.WebAPI.Validators;

public sealed class RegisterRequestValidator : AbstractValidator<RegisterRequest>
{
    public RegisterRequestValidator(UserManager<User> userManager)
    {
        RuleFor(r => r.UserName)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .Length(3, 100)
            .MustAsync(async (userName, _) =>
            {
                var existedUser = await userManager.FindByNameAsync(userName);
                return existedUser is null;
            })
            .WithMessage("A user with the same name already exists");
    }
}