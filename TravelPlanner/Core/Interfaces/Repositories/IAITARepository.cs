using TravelPlanner.Core.Entities;

namespace TravelPlanner.Core.Interfaces.Repositories
{
    public interface IAITARepository: IRepository<AITA>
    {
        Task<AITA?> GetAITACodeAsync(string city);
        Task SaveCodeAsync(AITA aita);
    }
}
