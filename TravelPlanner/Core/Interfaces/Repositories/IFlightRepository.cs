using TravelPlanner.Core.Entities;

namespace TravelPlanner.Core.Interfaces.Repositories
{
    public interface IFlightRepository: IRepository<Flight>
    {
        Task<IEnumerable<Flight>> GetSameFlightInfoAsync(DateTime departureDate, string origin, string destination);
        Task SaveChangesAsync();
        Task<IEnumerable<Flight>> GetFlightsByDepartureDateAsync(DateTime departureDate);
        Task<IEnumerable<Flight>> GetFlightsByOriginAsync(string origin);
        Task<IEnumerable<Flight>> GetFlightsByDestinationAsync(string destination);
        Task<IEnumerable<Flight>> GetFlightsByAirlineAsync(string airline);
    }
}
