using TravelPlanner.Core.Entities;

namespace TravelPlanner.Core.Interfaces.Repositories
{
    public interface IHotelRepository : IRepository<Hotel>
    {
        Task<List<Hotel>> GetHotelsByCityAndDate(string city, DateTime arrival, DateTime departure);
        Task SaveChangesAsync();
    }
}
