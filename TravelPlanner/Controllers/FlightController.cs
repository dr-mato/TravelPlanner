using Microsoft.AspNetCore.Mvc;
using TravelPlanner.Application.DTOs;
using TravelPlanner.Core.Interfaces.Services;

namespace TravelPlanner.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FlightController : ControllerBase
    {
        private readonly IFlightService _flightService;

        public FlightController(IFlightService flightService)
        {
            _flightService = flightService;
        }

        [HttpPost]
        public async Task<IActionResult> GetFlights([FromBody] FlightRequest request)
        {
            var flightData = await _flightService.GetFlightsAsync(request);
            return Ok(flightData);
        }
    }
}
