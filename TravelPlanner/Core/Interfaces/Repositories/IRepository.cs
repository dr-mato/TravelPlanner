namespace TravelPlanner.Core.Interfaces.Repositories
{
    public interface IRepository<TEntity>
    {
        Task<IEnumerable<TEntity>> GetAllAsync();

        Task<TEntity> GetByIdAsync(int id);

        Task AddAsync(TEntity entity);

        Task DeleteAsync(TEntity entity);

        Task DeleteByIdAsync(int id);

        Task UpdateAsync(TEntity entity);
    }
}
