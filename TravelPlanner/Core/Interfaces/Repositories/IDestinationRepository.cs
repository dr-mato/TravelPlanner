using TravelPlanner.Core.Entities;

namespace TravelPlanner.Core.Interfaces.Repositories
{
    public interface IDestinationRepository : IRepository<Destination>
    {
        Task<List<Destination>> GetDestinationByNameAsync(string name);
        Task<List<Destination>> GetDestinationByCountryAsync(string country);
        Task<(string name, int amount)> GetTheMostPopularDestinationAsync();
        Task<Destination> GetTheMostExpensiveDestinationAsync();
        Task<Destination> GetTheMostExpensiveDestinationByCoutryAsync(string country);
        Task SaveChangesAsync();
    }
}
