using Microsoft.EntityFrameworkCore;
using TravelPlanner.Core.Entities;
using TravelPlanner.Core.Interfaces.Repositories;
using TravelPlanner.Infrastructure.Data;

namespace TravelPlanner.Infrastructure.Repositories
{
    public class FlightRepository : Repository<Flight>, IFlightRepository
    {
        public FlightRepository(DataDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Flight>> GetSameFlightInfoAsync(DateTime departureDate, string origin, string destination)
        {
            return await _context.Set<Flight>()
                .Where(f => f.DepartureDate == departureDate && f.Origin == origin && f.Destination == destination)
                .ToListAsync();
        }

        public Task<IEnumerable<Flight>> GetFlightsByDepartureDateAsync(DateTime departureDate)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Flight>> GetFlightsByOriginAsync(string origin)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Flight>> GetFlightsByDestinationAsync(string destination)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Flight>> GetFlightsByAirlineAsync(string airline)
        {
            throw new NotImplementedException();
        }
    }
}
