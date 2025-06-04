using Microsoft.EntityFrameworkCore;
using TravelPlanner.Core.Entities;
using TravelPlanner.Core.Interfaces.Repositories;
using TravelPlanner.Infrastructure.Data;

namespace TravelPlanner.Infrastructure.Repositories
{
    public class FlightTwoRepository : Repository<FlightTwo>, IFlightTwoRepository
    {
        public FlightTwoRepository(DataDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<FlightTwo>> GetFlightsTwoInfoAsync(DateTime date, string origin, string destination,
            int passengers, FlightTwoClass flightClass)
        {
            return await _context.FlightTwos.Where(flight => flight.Date == date && flight.OriginCity == origin &&
            flight.DestinationCity == destination && flight.Passengers == passengers && flight.Class == flightClass).ToListAsync();
        }

        public async Task SaveFlightAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
