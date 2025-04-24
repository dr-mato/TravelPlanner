using Microsoft.EntityFrameworkCore;
using TravelPlanner.Core.Entities;
using TravelPlanner.Core.Interfaces.Repositories;
using TravelPlanner.Infrastructure.Data;

namespace TravelPlanner.Infrastructure.Repositories
{
    public class AirportInformationRepository : Repository<AirportInformation>, IAirportInformationRepository
    {
        public AirportInformationRepository(DataDbContext context) : base(context)
        {
        }

        public async Task<AirportInformation> GetAirportCodeAndIdAsync(string city)
        {
            return await _context.Set<AirportInformation>().Where(airport => airport.City == city).FirstOrDefaultAsync();
        }
    }
}
