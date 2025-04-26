using TravelPlanner.Core.Entities;

namespace TravelPlanner.Core.Interfaces.Repositories
{
    public interface IHotelRepository : IRepository<Hotel>
    {
        Task<List<Hotel>> GetHotelsByCity(string city);
        Task SaveChangesAsync();
    }
}
