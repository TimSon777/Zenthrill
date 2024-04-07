using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using OpenIddict.Validation.AspNetCore;
using Zenthrill.Settings.DependencyInjection;
using Zenthrill.WebAPI.Settings;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection;

public static class AuthorizationConfiguration
{
    public static IHostApplicationBuilder AddAuthorizationConfiguration(this IHostApplicationBuilder builder)
    {
        builder.ConfigureSettings<IdentityServerSettings>();
        var settings = builder.GetSettings<IdentityServerSettings>();

        builder.Services
            .AddOpenIddict()
            .AddValidation(options =>
            {
                options.SetIssuer(settings.Uri);
                options.UseSystemNetHttp();
                options.UseAspNetCore();
                options.Configure(a => a.TokenValidationParameters.IssuerSigningKey =
                    new SymmetricSecurityKey(
                        Convert.FromBase64String(settings.SingingKey)));
            });

        builder.Services.AddAuthentication(options =>
            options.DefaultScheme = OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme);

        builder.Services.AddAuthorization();

        return builder;
    }
}