using Microsoft.AspNetCore.Mvc;
using TravelPlanner.Application.DTOs;
using TravelPlanner.Core.Interfaces.Services;
using TravelPlanner.Infrastructure.Repositories;

namespace TravelPlanner.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AITAController : ControllerBase
    {
        private readonly IAITAService _aitaService;

        public AITAController(IAITAService aitaService)
        {
            _aitaService = aitaService;
        }

        [HttpPost]
        public async Task<IActionResult> GetAITACode(AITARequest request)
        {
            var aitaCode = await _aitaService.GetAITACodeAsync(request);
            return Ok(aitaCode);
        }
    }
}
