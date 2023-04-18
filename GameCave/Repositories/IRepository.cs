using GameCave.Data;

namespace GameCave.Repositories
{
    public interface IRepository<T> where T:BaseEntity
    {
        Task<T> GetByIdAsync(int id);
        Task<ICollection<T>> GetAllAsync();
        Task<T> CreateAsync(T item);
        Task<T> UpdateAsync(T item);
        Task<T> DeleteAsync(int id);
    }
}
