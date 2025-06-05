using Microsoft.AspNetCore.Mvc;
using TravelPlanner.Application.DTOs;
using TravelPlanner.Core.Interfaces.Services;

namespace TravelPlanner.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HotelTwoController: ControllerBase
    {
        private readonly IHotelTwoService _hotelTwoService;

        public HotelTwoController(IHotelTwoService hotelTwoService)
        {
            _hotelTwoService = hotelTwoService;
        }

        [HttpPost]
        public async Task<IActionResult> GetHotels([FromBody] HotelTwoRequest request)
        {
            var hotels = await _hotelTwoService.GetHotelsAsync(request);
            return Ok(hotels);
        }
    }
}
