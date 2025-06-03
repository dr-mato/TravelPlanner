using TravelPlanner.Core.Entities;

namespace TravelPlanner.Core.Interfaces.Repositories
{
    public interface IAmadeusTokenRepository: IRepository<AmadeusToken>
    {
        Task<AmadeusToken?> GetCurrentTokenAsync();
        Task SaveTokenAsync();
    }
}
