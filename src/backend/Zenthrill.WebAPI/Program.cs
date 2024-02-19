using Zenthrill.WebAPI.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.AddGraphDatabaseConfiguration();
builder.AddApplicationDbContextConfiguration();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
    options.CustomSchemaIds(x => x.FullName));

builder.Services.AddDateTimeOffsetProvider();
builder.Services.AddFeaturesValidators();
builder.Services.AddLabelsConstructor();
builder.Services.AddWebApiMappers();

builder.Services
    .AddReadStoryFeature()
    .AddCreateStoryFeature()
    .AddCreateBranchFeature()
    .AddCreateFragmentFeature();

var app = builder.Build();

app.UseApi(typeof(Zenthrill.WebAPI.AssemblyInfo).Assembly);

app.UseSwagger();
app.UseSwaggerUI();

app.Run();
