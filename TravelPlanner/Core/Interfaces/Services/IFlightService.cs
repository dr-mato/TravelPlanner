using System.Text.Json;
using TravelPlanner.Application.DTOs;
using TravelPlanner.Core.Entities;

namespace TravelPlanner.Core.Interfaces.Services
{
    public interface IFlightService
    {
        Task<IEnumerable<Flight>> GetFlightsAsync(FlightRequest flight);
    }
}
