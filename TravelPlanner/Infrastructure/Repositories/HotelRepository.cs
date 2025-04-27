using Microsoft.EntityFrameworkCore;
using TravelPlanner.Core.Entities;
using TravelPlanner.Core.Interfaces.Repositories;
using TravelPlanner.Infrastructure.Data;

namespace TravelPlanner.Infrastructure.Repositories
{
    public class HotelRepository : Repository<Hotel>, IHotelRepository
    {

        public HotelRepository(DataDbContext context) : base(context)
        {
        }

        public async Task<List<Hotel>> GetHotelsByCityAndDate(string city, DateTime arrival, DateTime departure)
        {
            return await _context.Set<Hotel>()
                .Where(h => h.City == city && h.CheckInDate == arrival && h.CheckOutDate == departure)
                .ToListAsync();
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
