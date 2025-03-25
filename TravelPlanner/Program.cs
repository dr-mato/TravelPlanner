using TravelPlanner.Application.Services;
using TravelPlanner.Core.Interfaces.Services;

var builder = WebApplication.CreateBuilder(args);

// Add layers
builder.Services.AddScoped<IRecommendationService, OpenAIService>();

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