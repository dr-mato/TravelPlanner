using TravelPlanner.Application.DTOs;
using TravelPlanner.Core.Entities;

namespace TravelPlanner.Core.Interfaces.Repositories
{
    public interface IFlightTwoRepository : IRepository<FlightTwo>
    {
        Task<IEnumerable<FlightTwo>> GetFlightsTwoInfoAsync(DateTime date, string origin, string destination, int passengers, FlightTwoClass flightClass);
        Task SaveFlightAsync();
    }
}
