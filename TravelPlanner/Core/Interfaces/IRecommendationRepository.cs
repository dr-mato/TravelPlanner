using TravelPlanner.Application.DTOs;
using TravelPlanner.Core.Entities;

namespace TravelPlanner.Core.Interfaces
{
    public interface IRecommendationRepository
    {
        Task<List<Destination>> GetRecommendationsAsync(UserPreferences preferences);
    }
}
