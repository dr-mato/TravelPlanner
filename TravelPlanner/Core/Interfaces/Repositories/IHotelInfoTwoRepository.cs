using TravelPlanner.Application.DTOs;
using TravelPlanner.Core.Entities;

namespace TravelPlanner.Core.Interfaces.Repositories
{
    public interface IHotelInfoTwoRepository : IRepository<HotelInfoTwo>
    {   
        Task<List<HotelInfoTwo>> GetHotels(HotelListTwoRequest hotelRequirements);
        Task SaveHotelInfoAsync();
    }
}
