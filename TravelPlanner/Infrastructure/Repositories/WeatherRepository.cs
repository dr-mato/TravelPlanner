using TravelPlanner.Core.Entities;
using TravelPlanner.Core.Interfaces.Repositories;

namespace TravelPlanner.Infrastructure.Repositories
{
    public class WeatherRepository : IWeatherRepository
    {
        public Task AddWeatherDataAsync(List<Weather> data)
        {
            throw new NotImplementedException();
        }

        public Task<List<Weather>> GetExistingWeatherAsync(string location, DateTime startDate, DateTime endDate)
        {
            throw new NotImplementedException();
        }

        public Task<bool> HasAllDataForRangeAsync(string location, DateTime startDate, DateTime endDate)
        {
            throw new NotImplementedException();
        }
    }
}
