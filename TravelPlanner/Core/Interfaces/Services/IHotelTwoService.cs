using TravelPlanner.Application.DTOs;
using TravelPlanner.Core.Entities;

namespace TravelPlanner.Core.Interfaces.Services
{
    public interface IHotelTwoService
    {
        Task<IEnumerable<HotelTwo>> GetHotelsAsync(HotelTwoRequest request);
    }
}
