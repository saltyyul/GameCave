using GameCave.Data;
using GameCave.Repositories;
using Microsoft.AspNetCore.Identity;

namespace GameCave.Services
{
    public class GenreService : IGenreService
    {
        private readonly IRepository<Genre> _genreRepository;
        private readonly UserManager<IdentityUser> _userManager;

        public GenreService(IRepository<Genre> genreRepository, UserManager<IdentityUser> userManager)
        {
            _genreRepository = genreRepository;
            _userManager = userManager;
        }
        public async Task<Genre> CreateAsync(Genre genre, string userId)
        {
            genre.Created = DateTime.Now;
            genre.Modified = DateTime.Now;
            genre.CreatedById = userId;
            genre.ModifiedById = userId;

            return await _genreRepository.CreateAsync(genre);
        }

        public async Task<Genre> DeleteAsync(int genreId)
        {
            return await _genreRepository.DeleteAsync(genreId);
        }

        public async Task<ICollection<Genre>> GetAllGenresAsync()
        {
            return await _genreRepository.GetAllAsync();
        }

        public async Task<Genre> GetGenreByIdAsync(int genreId)
        {
            return await _genreRepository.GetByIdAsync(genreId);
        }

        public async Task<Genre> UpdateAsync(Genre genre, string userId)
        {
            genre.Modified = DateTime.Now;
            genre.ModifiedById = userId;
            return await _genreRepository.UpdateAsync(genre);
        }
    
    }
}
