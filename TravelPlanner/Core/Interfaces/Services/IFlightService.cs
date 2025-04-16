using TravelPlanner.Core.Entities;

namespace TravelPlanner.Core.Interfaces.Services
{
    public interface IFlightService
    {
        Task<IEnumerable<Flight>> GetFlightsAsync(string origin, string destination, DateTime departureDate, DateTime returnDate);
    }
}
