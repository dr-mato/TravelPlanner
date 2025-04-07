using Microsoft.EntityFrameworkCore;
using TravelPlanner.Application.Services;
using TravelPlanner.Core.Interfaces.Services;
using TravelPlanner.Infrastructure.Data;

var builder = WebApplication.CreateBuilder(args);

// Add layers
builder.Services.AddScoped<IOpenAIService, OpenAIService>();

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

builder.Services.AddDbContext<DataDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

app.UseHttpsRedirection();
app.MapControllers();
app.Run();