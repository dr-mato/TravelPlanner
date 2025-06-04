using TravelPlanner.Application.DTOs;
using TravelPlanner.Core.Entities;

namespace TravelPlanner.Core.Interfaces.Services
{
    public interface IHotelListTwoService
    {
        Task<IEnumerable<HotelInfoTwo>> GetHotelInfoByCityAsync(HotelListTwoRequest request);
    }
}
