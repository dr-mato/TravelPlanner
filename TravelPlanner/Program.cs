using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;
using TravelPlanner.Application.Services;
using TravelPlanner.Core.Interfaces.Repositories;
using TravelPlanner.Core.Interfaces.Services;
using TravelPlanner.Infrastructure.Data;
using TravelPlanner.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Configure CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.WithOrigins("http://localhost:8080") // Your Vite frontend origin
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

builder.Services.AddDbContext<DataDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });

builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddScoped<IDestinationRepository, DestinationRepository>();
builder.Services.AddScoped<IWeatherRepository, WeatherRepository>();
builder.Services.AddScoped<IAirportInformationRepository, AirportInformationRepository>();
builder.Services.AddScoped<IFlightRepository, FlightRepository>();
builder.Services.AddScoped<IHotelRepository, HotelRepository>();
builder.Services.AddScoped<IHotelDestinationRepository, HotelDestinationRepository>();
builder.Services.AddScoped<IAmadeusTokenRepository, AmadeusTokenRepository>();
builder.Services.AddScoped<IAITARepository, AITARepository>();
builder.Services.AddScoped<IFlightTwoRepository, FlightTwoRepository>();
builder.Services.AddScoped<IHotelInfoTwoRepository, HotelInfoTwoRepository>();
builder.Services.AddScoped<IHotelTwoRepository, HotelTwoRepository>();

builder.Services.AddScoped<IOpenAIService, OpenAIService>();
builder.Services.AddScoped<IWeatherService, WeatherService>();
builder.Services.AddScoped<IFlightService, FlightService>();
builder.Services.AddScoped<IHotelService, HotelService>();
builder.Services.AddScoped<IAmadeusTokenGenerationService, AmadeusTokenGenerationService>();
builder.Services.AddScoped<IAITAService, AITAService>();
builder.Services.AddScoped<IFlightTwoService, FlightTwoService>();
builder.Services.AddScoped<IHotelListTwoService, HotelListTwoService>();
builder.Services.AddScoped<IHotelTwoService, HotelTwoService>();   

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

// Enable CORS
app.UseCors("AllowFrontend");

app.MapControllers();
await app.RunAsync();