using TravelPlanner.Application.DTOs;
using TravelPlanner.Core.Entities;

namespace TravelPlanner.Core.Interfaces.Services
{
    public interface IFlightTwoService
    {
        Task<IEnumerable<FlightTwo>> GetFlightsTwoAsync(FlightTwoRequest request);
    }
}
