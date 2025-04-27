using Microsoft.AspNetCore.Mvc;
using TravelPlanner.Application.DTOs;
using TravelPlanner.Core.Interfaces.Services;

namespace TravelPlanner.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OpenAIController : ControllerBase
    {
        private readonly IOpenAIService _service;
        public OpenAIController(IOpenAIService service)
            => _service = service;

        [HttpPost]
        public async Task<IActionResult> GetRecommendations([FromBody] UserPreferences preferences)
        {
            var destinations = await _service.GetRecommendationsAsync(preferences);
            return Ok(destinations);
        }
    }
}
