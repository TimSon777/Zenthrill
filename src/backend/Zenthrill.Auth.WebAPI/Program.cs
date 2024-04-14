using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Net.Http.Headers;
using Microsoft.OpenApi.Models;
using Zenthrill.Auth.Model.Entities;
using Zenthrill.Auth.Model.Infrastructure.EntityFrameworkCore;
using Zenthrill.Auth.WebAPI.Requests;
using Zenthrill.Auth.WebAPI.Validators;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();

var info = new OpenApiInfo
{
};
        
var scheme = new OpenApiSecurityScheme
{
    Name = HeaderNames.Authorization,
    Type = SecuritySchemeType.OAuth2,
    Scheme = "Bearer",
    BearerFormat = JwtConstants.TokenType,
    In = ParameterLocation.Header,
            
    Flows = new OpenApiOAuthFlows
    {
        Password = new OpenApiOAuthFlow
        {
            TokenUrl = new Uri("http://localhost:5273/connect/token")
        }
    }
};

var securityRequirement = new OpenApiSecurityRequirement();
var securityScheme = new OpenApiSecurityScheme
{
    Reference = new OpenApiReference
    {
        Type = ReferenceType.SecurityScheme,
        Id = "oauth2"
    }
};
        
securityRequirement.Add(securityScheme, Array.Empty<string>());
        
builder.Services.AddSwaggerGen(o =>
{
    o.SwaggerDoc("v1", info);
    o.AddSecurityDefinition("oauth2", scheme);
    o.AddSecurityRequirement(securityRequirement);
    o.CustomSchemaIds(t => t.ToString());
});

builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    var connectionString = builder.Configuration["DATABASE_CONNECTION_STRING"];
    options.UseNpgsql(connectionString);
    options.UseOpenIddict();
});

builder.Services.AddIdentity<User, Role>()
    .AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services
    .AddOpenIddict()
    .AddCore(options =>
        options
            .UseEntityFrameworkCore()
            .UseDbContext<ApplicationDbContext>())
    .AddServer(options =>
        options
            .SetAccessTokenLifetime(TimeSpan.FromDays(365))
            .AllowPasswordFlow()
            .SetIssuer("http://identity_server:80")
            .SetTokenEndpointUris("/connect/token")
            .AcceptAnonymousClients()
            .UseAspNetCore(openIddictServerAspNetCoreBuilder =>
                openIddictServerAspNetCoreBuilder
                    .DisableTransportSecurityRequirement()
                    .EnableTokenEndpointPassthrough())
            .AddSigningKey(new SymmetricSecurityKey(
                Convert.FromBase64String("c2ZudXdkaXV2Y2Job2l3dm5pdXdldmJvd2I0M3k3ODJnM2Nz")))
            .AddDevelopmentSigningCertificate()
            .AddDevelopmentEncryptionCertificate()
            .DisableAccessTokenEncryption())
    .AddValidation(options =>
    {
        options.UseAspNetCore();
        options.UseLocalServer();
    });

builder.Services.AddControllers();

builder.Services.AddScoped<IValidator<RegisterRequest>, RegisterRequestValidator>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapControllers();
app.Run();
