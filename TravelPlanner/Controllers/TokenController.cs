using Microsoft.AspNetCore.Mvc;
using TravelPlanner.Core.Interfaces.Services;

namespace TravelPlanner.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TokenController : ControllerBase
    {
        private readonly IAmadeusTokenGenerationService _tokenGenerationService;

        public TokenController(IAmadeusTokenGenerationService tokenGenerationService)
        {
            _tokenGenerationService = tokenGenerationService;
        }

        [HttpPost]
        public async Task<IActionResult> GenerateToken()
        {
            var token = await _tokenGenerationService.GenerateAmadeusTokenAsync();
            return Ok(token);
        }
    }
}
