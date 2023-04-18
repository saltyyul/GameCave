using GameCave.Data;

namespace GameCave.Services
{
    public interface IGenreService
    {
        public Task<Genre> GetGenreByIdAsync(int genreId);
        public Task<ICollection<Genre>> GetAllGenresAsync();
        public Task<Genre> CreateAsync(Genre genre, string userId);
        public Task<Genre> UpdateAsync(Genre genre, string userId);
        public Task<Genre> DeleteAsync(int genreId);
    }
}
