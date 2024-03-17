using System.Text.Json.Serialization;
using Zenthrill.WebAPI.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.AddGraphDatabaseConfiguration();
builder.AddApplicationDbContextConfiguration();
builder.AddS3Configuration();

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
    .AddFragmentFeatures();

var app = builder.Build();

app.UseApi(typeof(Zenthrill.WebAPI.AssemblyInfo).Assembly);

app.UseSwagger();
app.UseSwaggerUI();

app.Run();
