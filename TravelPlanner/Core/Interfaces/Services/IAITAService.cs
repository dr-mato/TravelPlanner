using TravelPlanner.Application.DTOs;

namespace TravelPlanner.Core.Interfaces.Services
{
    public interface IAITAService
    {
        Task<string> GetAITACodeAsync(AITARequest request);
    }
}
