using TravelPlanner.Application.DTOs;
using TravelPlanner.Core.Entities;

namespace TravelPlanner.Core.Interfaces.Services
{
    public interface IWeatherService
    {
        Task<List<Weather>> GetWeatherRangeForLocationAsync(WeatherRequest request);
    }
}
