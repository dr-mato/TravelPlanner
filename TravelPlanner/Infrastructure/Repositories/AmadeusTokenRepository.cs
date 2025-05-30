using Microsoft.EntityFrameworkCore;
using TravelPlanner.Core.Entities;
using TravelPlanner.Core.Interfaces.Repositories;
using TravelPlanner.Infrastructure.Data;

namespace TravelPlanner.Infrastructure.Repositories
{
    public class AmadeusTokenRepository : Repository<AmadeusToken>, IAmadeusTokenRepository
    {
        public AmadeusTokenRepository(DataDbContext context) : base(context)
        {
        }

        public async Task<AmadeusToken?> GetCurrentTokenAsync()
        {
            return await _context.AmadeusTokens.FirstOrDefaultAsync();
        }

        public async Task SaveTokenAsync(AmadeusToken token)
        {
            await _context.AmadeusTokens.AddAsync(token);
        }
    }
}
