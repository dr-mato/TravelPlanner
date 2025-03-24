using TravelPlanner.Infrastructure.Repositories;
using TravelPlanner.Application.Services;
using TravelPlanner.Core.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add layers
builder.Services.AddScoped<IRecommendationRepository, OpenAIRepository>();
builder.Services.AddScoped<RecommendationService>();

builder.Services.AddHttpClient();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.MapControllers();
app.Run();