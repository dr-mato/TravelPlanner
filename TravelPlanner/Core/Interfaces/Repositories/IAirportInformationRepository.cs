using TravelPlanner.Core.Entities;

namespace TravelPlanner.Core.Interfaces.Repositories
{
    public interface IAirportInformationRepository : IRepository<AirportInformation>
    {
        Task<AirportInformation> GetAirportCodeAndIdAsync(string city);
    }
}
