using TravelPlanner.Core.Entities;

namespace TravelPlanner.Core.Interfaces.Repositories
{
    public interface IHotelDestinationRepository : IRepository<HotelDestination>
    {
        Task<HotelDestination> GetHotelInformationByCity(string city);
        Task SaveChangesAsync();
    }
}
