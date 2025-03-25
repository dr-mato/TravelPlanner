using TravelPlanner.Application.DTOs;
using TravelPlanner.Core.Entities;

namespace TravelPlanner.Core.Interfaces.Services
{
    public interface IRecommendationService
    {
        Task<List<Destination>> GetRecommendationsAsync(UserPreferences preferences);
    }
}
