using TravelPlanner.Core.Entities;
using TravelPlanner.Core.Interfaces;
using TravelPlanner.Application.DTOs;

namespace TravelPlanner.Application.Services;

public class RecommendationService
{
    private readonly IRecommendationRepository _repository;

    public RecommendationService(IRecommendationRepository repository)
    {
        _repository = repository;
    }

    public async Task<List<Destination>> GetRecommendationsAsync(UserPreferences preferences)
    {
        return await _repository.GetRecommendationsAsync(preferences);
    }
}