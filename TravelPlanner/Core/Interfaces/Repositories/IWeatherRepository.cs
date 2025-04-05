using TravelPlanner.Core.Entities;

namespace TravelPlanner.Core.Interfaces.Repositories
{
    public interface IWeatherRepository
    {
        Task<List<Weather>> GetExistingWeatherAsync(string location, DateTime startDate, DateTime endDate);
        Task AddWeatherDataAsync(List<Weather> data);
        Task<bool> HasAllDataForRangeAsync(string location, DateTime startDate, DateTime endDate);
    }
}
