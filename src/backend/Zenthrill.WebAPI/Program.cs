using Zenthrill.WebAPI.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.AddGraphDatabaseConfiguration();
builder.AddApplicationDbContextConfiguration();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDateTimeOffsetProvider();
builder.Services.AddFeaturesValidators();

builder.Services
    .AddCreateStoryFeature();

var app = builder.Build();

app.UseApi(typeof(Zenthrill.WebAPI.AssemblyInfo).Assembly);

app.UseSwagger();
app.UseSwaggerUI();

app.Run();
