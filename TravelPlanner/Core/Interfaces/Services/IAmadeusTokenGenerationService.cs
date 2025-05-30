using TravelPlanner.Core.Entities;

namespace TravelPlanner.Core.Interfaces.Services
{
    public interface IAmadeusTokenGenerationService
    {
        Task<AmadeusToken> GenerateAmadeusTokenAsync();
    }
}
