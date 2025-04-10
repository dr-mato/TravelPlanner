using Microsoft.EntityFrameworkCore;
using TravelPlanner.Core.Entities;
using TravelPlanner.Core.Interfaces.Repositories;
using TravelPlanner.Infrastructure.Data;

namespace TravelPlanner.Infrastructure.Repositories
{
    public class WeatherRepository : Repository<Weather>, IWeatherRepository
    {
        public WeatherRepository(DataDbContext context) : base(context) { }

        public async Task AddWeatherDataAsync(List<Weather> data)
        {
            foreach(var weather in data)
            {
                var existingWeather = await _context.Set<Weather>().
                    FirstOrDefaultAsync(w => w.Location == weather.Location && w.Date == weather.Date);

                if (existingWeather != null)
                {
                    existingWeather.Temperature = weather.Temperature;
                    Update(existingWeather);
                }
                else
                {
                    await AddAsync(weather);
                }
            }
            await _context.SaveChangesAsync();
        }

        public async Task<List<Weather>> GetExistingWeatherAsync(string location, DateTime startDate, DateTime endDate)
        {
            return await _context.Set<Weather>().Where(d => d.Location == location && d.Date >= startDate && d.Date <= endDate).ToListAsync();
        }

        public async Task<bool> HasAllDataForRangeAsync(string location, DateTime startDate, DateTime endDate)
        {
            var date = startDate;

            while(date <= endDate)
            {
                var weather = await _context.Set<Weather>().Where(d => d.Location == location && d.Date == date).FirstOrDefaultAsync();
                if (weather == null)
                {
                    return false;
                }
                date = date.AddDays(1);
            }

            return true;
        }
    }
}
