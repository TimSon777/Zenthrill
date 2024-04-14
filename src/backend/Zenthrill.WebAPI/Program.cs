using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using Zenthrill.Application.Settings;
using Zenthrill.Settings.DependencyInjection;
using Zenthrill.WebAPI.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.AddGraphDatabaseConfiguration();
builder.AddApplicationDbContextConfiguration();
builder.AddS3Configuration();
builder.AddAuthorizationConfiguration();
builder.AddIdentityProviderClientConfiguration();
builder.AddBackgroundServices();
builder.AddCors();

builder.Services.AddEndpointsApiExplorer();
builder.Services.ConfigureHttpJsonOptions(o =>
{
    o.SerializerOptions.Converters.Add(new JsonStringEnumConverter());
});

builder.Services.Configure<Microsoft.AspNetCore.Mvc.JsonOptions>(options =>
{
    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
});

builder.Services.AddSwaggerGen(options =>
    options.CustomSchemaIds(x => x.FullName));

builder.Services.AddDateTimeOffsetProvider();
builder.Services.AddFeaturesValidators();
builder.Services.AddWebApiMappers();
builder.Services.AddLocalizerFactory();

builder.Services
    .AddReadStoryFeature()
    .AddCreateStoryFeature()
    .AddCreateExampleStoryFeature()
    .AddCreateBranchFeature()
    .AddUpdateBranchFeature()
    .AddGetUploadFileLinkFeature()
    .AddFragmentFeatures()
    .AddFeatures();

var app = builder.Build();

app.UseCors("Frontend");
app.UseAuthentication();
app.UseAuthorization();
app.UseApi(typeof(Zenthrill.WebAPI.AssemblyInfo).Assembly);

app.UseSwagger();
app.UseSwaggerUI();

app.Run();
