using Microsoft.AspNetCore.Mvc;
using TravelPlanner.Application.DTOs;
using TravelPlanner.Core.Interfaces;

namespace TravelPlanner.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RecommendationsController : ControllerBase
    {
        private readonly IRecommendationRepository _repository;
        public RecommendationsController(IRecommendationRepository repository)
            => _repository = repository;

        [HttpPost]
        public async Task<IActionResult> GetRecommendations([FromBody] UserPreferences preferences)
        {
            var destinations = await _repository.GetRecommendationsAsync(preferences);
            return Ok(destinations);
        }
    }
}
