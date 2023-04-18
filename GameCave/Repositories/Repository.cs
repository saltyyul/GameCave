using GameCave.Data;
using Microsoft.EntityFrameworkCore;

namespace GameCave.Repositories
{
    public class Repository<T> : IRepository<T> where T : BaseEntity
    {
        private readonly ApplicationDbContext _context;

        public Repository(ApplicationDbContext context)
        {
            _context = context;
        }

        public virtual async Task<T> CreateAsync(T item)
        {
            _context.Set<T>().Add(item);
            await _context.SaveChangesAsync();

            return item;
        }

        public virtual async Task<T> DeleteAsync(int id)
        {
            var item = await GetByIdAsync(id);
            _context.Remove(item);
            await _context.SaveChangesAsync();
            return item;
        }

        public virtual async Task<ICollection<T>> GetAllAsync()
        {
            return await _context.Set<T>().ToListAsync();
        }

        public virtual async Task<T> GetByIdAsync(int id)
        {
            return await _context.Set<T>()
                .FindAsync(id);
        }

        public virtual async Task<T> UpdateAsync(T item)
        {
            if (_context.Set<T>() != null)
            {
                _context.Set<T>().Update(item);
                _context.Entry(item).Property(x => x.CreatedById).IsModified = false;
                _context.Entry(item).Property(x => x.Created).IsModified = false;
                await _context.SaveChangesAsync();
            }
            return item;
        }
    }
}
