using Microsoft.AspNetCore.Mvc;
using TravelPlanner.Application.DTOs;
using TravelPlanner.Core.Interfaces.Services;

namespace TravelPlanner.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FlightTwoController : ControllerBase
    {
        private readonly IFlightTwoService _flightService;

        public FlightTwoController(IFlightTwoService flightService)
        {
            _flightService = flightService;
        }

        [HttpPost]
        public async Task<IActionResult> GetFlightsTwo(FlightTwoRequest request)
        {
            var flightDetails = await _flightService.GetFlightsTwoAsync(request);
            return Ok(flightDetails);
        }
    }
}
