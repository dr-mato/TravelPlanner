using Microsoft.Identity.Client;
using TravelPlanner.Application.DTOs;
using TravelPlanner.Core.Entities;

namespace TravelPlanner.Core.Interfaces.Repositories
{
    public interface IHotelTwoRepository: IRepository<HotelTwo>
    {
        Task<List<HotelTwo>> GetHotels(HotelTwoRequest info);
        Task SaveHotelsAsync();
    }
}
