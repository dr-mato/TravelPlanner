using TravelPlanner.Core.Entities;
using TravelPlanner.Core.Interfaces.Repositories;

namespace TravelPlanner.Infrastructure.Repositories
{
    public class DestinationRepository : IDestinationRepository
    {
        public Task<List<Destination>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<Destination> GetDestinationByCountryAsync(string country)
        {
            throw new NotImplementedException();
        }

        public Task<Destination> GetDestinationByNameAsync(string name)
        {
            throw new NotImplementedException();
        }

        public Task<Destination> GetTheMostExpensiveDestinationAsync()
        {
            throw new NotImplementedException();
        }

        public Task<Destination> GetTheMostExpensiveDestinationByCoutryAsync(string country)
        {
            throw new NotImplementedException();
        }

        public Task<Destination> GetTheMostPopularDestinationAsync()
        {
            throw new NotImplementedException();
        }
    }
}
