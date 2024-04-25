using Microsoft.EntityFrameworkCore;
using Zenthrill.UserStory.Model.Clients;
using Zenthrill.UserStory.Model.Infrastructure.EntityFrameworkCore;
using Zenthrill.UserStory.Model.Services;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IUserProcessor, UserProcessor>();
builder.Services.AddScoped<IStoryRuntime, StoryRuntime>();
builder.Services.AddSingleton<IStoryClient, StoryClient>();

builder.Services.AddCors(options =>
    options.AddPolicy("Frontend",
        b => b.AllowAnyMethod()
            .AllowCredentials()
            .AllowAnyHeader()
            .WithOrigins("http://localhost:3000")));

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DATABASE")));

builder.Services.AddHttpClient("Story", options =>
{
    options.BaseAddress = new Uri(builder.Configuration["STORY_BASE_ADDRESS"]!);
});

var app = builder.Build();

app.UseCors("Frontend");
app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();