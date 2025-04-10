using Microsoft.AspNetCore.Mvc;
using TravelPlanner.Application.DTOs;
using TravelPlanner.Core.Interfaces.Services;

namespace TravelPlanner.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WeatherController : ControllerBase
    {
        private readonly IWeatherService _weatherService;

        public WeatherController(IWeatherService weatherService)
        {
            _weatherService = weatherService;
        }

        [HttpPost]
        public async Task<IActionResult> GetWeather([FromBody] WeatherRequest request)
        {
            var weatherData = await _weatherService.GetWeatherRangeForLocationAsync(request);
            return Ok(weatherData);
        }
    }
}
