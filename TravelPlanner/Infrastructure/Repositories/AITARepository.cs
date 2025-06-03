using Microsoft.EntityFrameworkCore;
using TravelPlanner.Core.Entities;
using TravelPlanner.Core.Interfaces.Repositories;
using TravelPlanner.Infrastructure.Data;

namespace TravelPlanner.Infrastructure.Repositories
{
    public class AITARepository : Repository<AITA>, IAITARepository
    {
        public AITARepository(DataDbContext context) : base(context)
        {
        }

        public Task<AITA?> GetAITACodeAsync(string city)
        {
            return _context.AITAs
                .FirstOrDefaultAsync(a => a.City.ToLower() == city.ToLower());
        }

        public async Task SaveCodeAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
