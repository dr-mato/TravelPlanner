using TravelPlanner.Application.DTOs;
using TravelPlanner.Core.Entities;

namespace TravelPlanner.Core.Interfaces.Services
{
    public interface IHotelService
    {
        public Task<List<Hotel>> GetHotelsAsync(HotelRequest request);
    }
}
