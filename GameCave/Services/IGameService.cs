using GameCave.Models.ViewModels.GameVMs;

namespace GameCave.Services
{
    public interface IGameService
    {
        public Task<GameViewModel> GetGameByIdAsync(int gameId);
        public Task<GameDetailsViewModel> GetGameDetailsByIdAsync(int gameId);
        public Task<ICollection<GameDetailsViewModel>> GetAllGamesAsync();
        public Task<GameViewModel> CreateAsync(GameViewModel game, string userId);
        public Task<GameViewModel> UpdateAsync(GameViewModel game, string userId);
        public Task<GameDetailsViewModel> DeleteAsync(int gameId);
    }
}
