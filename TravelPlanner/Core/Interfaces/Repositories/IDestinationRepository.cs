using TravelPlanner.Core.Entities;

namespace TravelPlanner.Core.Interfaces.Repositories
{
    public interface IDestinationRepository
    {
        Task<List<Destination>> GetAllAsync();
        Task<Destination> GetDestinationByNameAsync(string name);
        Task<Destination> GetDestinationByCountryAsync(string country);
        Task<Destination> GetTheMostPopularDestinationAsync();
        Task<Destination> GetTheMostExpensiveDestinationAsync();
        Task<Destination> GetTheMostExpensiveDestinationByCoutryAsync(string country);

    }
}
