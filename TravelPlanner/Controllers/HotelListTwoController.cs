using Microsoft.AspNetCore.Mvc;
using TravelPlanner.Application.DTOs;
using TravelPlanner.Core.Interfaces.Services;

namespace TravelPlanner.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HotelListTwoController : ControllerBase
    {
        private readonly IHotelListTwoService _hotelListTwoService;

        public HotelListTwoController(IHotelListTwoService hotelListTwoService)
        {
            _hotelListTwoService = hotelListTwoService;
        }

        [HttpPost]
        public async Task<IActionResult> GetHotels([FromBody] HotelListTwoRequest request)
        {
            var hotels = await _hotelListTwoService.GetHotelInfoByCityAsync(request);
            return Ok(hotels);
        }
    }
}
