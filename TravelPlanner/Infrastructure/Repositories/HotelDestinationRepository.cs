using Microsoft.EntityFrameworkCore;
using TravelPlanner.Core.Entities;
using TravelPlanner.Core.Interfaces.Repositories;
using TravelPlanner.Infrastructure.Data;

namespace TravelPlanner.Infrastructure.Repositories
{
    public class HotelDestinationRepository : Repository<HotelDestination>, IHotelDestinationRepository
    {
        public HotelDestinationRepository(DataDbContext context) : base(context)
        {
        }

        public async Task<HotelDestination> GetHotelInformationByCity(string city)
        {
            return await _context.Set<HotelDestination>().Where(hotel => hotel.City == city).FirstOrDefaultAsync();
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
