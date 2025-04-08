using Microsoft.EntityFrameworkCore;
using TravelPlanner.Core.Entities;
using TravelPlanner.Core.Interfaces.Repositories;
using TravelPlanner.Infrastructure.Data;

namespace TravelPlanner.Infrastructure.Repositories
{
    public class DestinationRepository : Repository<Destination>, IDestinationRepository
    {
        public DestinationRepository(DataDbContext dbContext) : base(dbContext) { }

        public async Task<List<Destination>> GetDestinationByCountryAsync(string country)
        {
            return await _context.Set<Destination>().Where(d => d.Country == country).ToListAsync()
                ?? throw new KeyNotFoundException($"The country you were looking for ({country}) was not in the database");
        }

        public async Task<List<Destination>> GetDestinationByNameAsync(string name)
        {
            return await _context.Set<Destination>().Where(d => d.Name == name).ToListAsync()
                ?? throw new KeyNotFoundException($"The destination you were looking for ({name}) was not in the database");
        }

        public async Task<Destination> GetTheMostExpensiveDestinationAsync()
        {
            return await _context.Set<Destination>()
                .OrderByDescending(d => d.BudgetEstimate)
                .FirstOrDefaultAsync()
                ?? throw new KeyNotFoundException("No destinations found in the database.");
        }

        public async Task<Destination> GetTheMostExpensiveDestinationByCoutryAsync(string country)
        {
            return await _context.Set<Destination>()
                .Where(d => d.Country == country)
                .OrderByDescending(d => d.BudgetEstimate)
                .FirstOrDefaultAsync()
                ?? throw new KeyNotFoundException($"The country you were looking for ({country}) was not in the database");
        }

        public async Task<(string name, int amount)> GetTheMostPopularDestinationAsync()
        {
            var popularDestination = await _context.Set<Destination>()
                .GroupBy(d => d.Name)
                .Select(g => new
                {
                    Name = g.Key,
                    Count = g.Count()
                })
                .OrderByDescending(g => g.Count)
                .FirstOrDefaultAsync()
                ?? throw new KeyNotFoundException("No destinations found in the database.");
            
            return (popularDestination.Name, popularDestination.Count);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
