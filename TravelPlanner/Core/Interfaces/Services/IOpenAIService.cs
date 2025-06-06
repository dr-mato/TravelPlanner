using TravelPlanner.Application.DTOs;
using TravelPlanner.Core.Entities;

namespace TravelPlanner.Core.Interfaces.Services
{
    public interface IOpenAIService
    {
        Task<List<Destination>> GetRecommendationsAsync(UserPreferences preferences);
        Task<List<DailyPlan>> GetDailyPlanAsync(DailyPlanRequest request);
        Task<List<DailyPlan>> GetDailyPlanTwoAsync(DailyPlanTwoRequest request);
        Task<List<DailyPlan>> GetDailyPlanThreeAsync(DailyPlanThreeRequest request);
    }
}
