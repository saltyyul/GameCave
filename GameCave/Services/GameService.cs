using GameCave.Data;
using GameCave.Models.ViewModels.GameVMs;
using GameCave.Repositories;
using Microsoft.AspNetCore.Identity;

namespace GameCave.Services
{
    public class GameService : IGameService
    {
        private readonly IRepository<Game> _gameRepository;
        private readonly IRepository<Genre> _genreRepository;
        private readonly IRepository<Review> _reviewRepository;
        private readonly IRepository<Company> _companyRepository;
        private readonly IWebHostEnvironment _env;
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        //private readonly IRepository<GameGenres> _gameGenresRepository;
        public GameService(IRepository<Game> gameRepository, IRepository<Genre> genreRepository, IRepository<Review> reviewRepository, IRepository<Company> companyRepository, IWebHostEnvironment environment, UserManager<IdentityUser> userManager, ApplicationDbContext context)
        {
            _gameRepository = gameRepository;
            _genreRepository = genreRepository;
            _reviewRepository = reviewRepository;
            _companyRepository = companyRepository;
            _env = environment;
            _context = context;
            _userManager = userManager;

        }
        public async Task<GameViewModel> CreateAsync(GameViewModel gameView, string userId)
        {
            //Converts the view into the actual entity
            Game game = new Game();
            game.Name = gameView.Name;
            game.Description = gameView.Description;
            game.CompanyID = gameView.CompanyID;
            game.Price = gameView.Price;
            game.Quantity = gameView.Quantity;
            game.Created = DateTime.Now;
            game.CreatedById = userId;
            game.CreatedBy = (await _userManager.FindByIdAsync(game.CreatedById));
            game.Modified = DateTime.Now;
            game.ModifiedById = userId;
            game.ModifiedBy = (await _userManager.FindByIdAsync(game.CreatedById));
            game.ImageURL = gameView.ImageURL;

            //Adds game in table and saves
            await _gameRepository.CreateAsync(game);

            int currentGameId = game.Id;

            //Adds the genres for the game in the GameGenres table
            if (gameView.GenreIds.Count > 0)
                foreach (var genreId in gameView.GenreIds)
                {
                    _context.GameGenres.Add(new GameGenres()
                    {
                        GameID = currentGameId,
                        GenreID = genreId
                    });

                    await _context.SaveChangesAsync();
                }

            return gameView;
        }

        public async Task<GameDetailsViewModel> DeleteAsync(int gameId)
        {
            Game game = await _gameRepository.GetByIdAsync(gameId);
            if (game != null)
            {
                GameDetailsViewModel gameView = new GameDetailsViewModel();
                gameView.Id = gameId;
                gameView.Name = game.Name;
                gameView.Description = game.Description;
                gameView.Price = game.Price;
                gameView.Quantity = game.Quantity;
                gameView.ImageURL = game.ImageURL;
                gameView.Company = game.Company;
                gameView.Reviews = game.Reviews;
                gameView.Genres = _context.GameGenres.Where(x => x.GameID.Equals(gameView.Id)).Select(y => y.Genre).ToList();


                //Ensure that the image is deleted from the root folder
                if (!string.IsNullOrEmpty(game.ImageURL))
                {
                    string url = game.ImageURL.Split('/', StringSplitOptions.RemoveEmptyEntries)[1].Trim();

                    var imageFullPath = string.Concat(Directory.GetCurrentDirectory(), "\\wwwroot\\", "image\\", url);
                    if (File.Exists(imageFullPath))
                    {
                        File.Delete(imageFullPath);
                    }
                }
                await _gameRepository.DeleteAsync(gameId);
                return gameView;
            }
            return null;
        }

        public async Task<ICollection<GameDetailsViewModel>> GetAllGamesAsync()
        {
            var games = await _gameRepository.GetAllAsync();
            ICollection<GameDetailsViewModel> result = new List<GameDetailsViewModel>();
            if (games.Count > 0)
            {

                foreach (var game in games)
                {
                    GameDetailsViewModel view = new GameDetailsViewModel();
                    view.Id = game.Id;
                    view.Name = game.Name;
                    view.Description = game.Description;
                    view.Price = game.Price;
                    view.Quantity = game.Quantity;
                    view.ImageURL = game.ImageURL;
                    view.Company = await _companyRepository.GetByIdAsync(game.CompanyID);
                    view.Reviews = game.Reviews;
                    view.Genres = _context.GameGenres.Where(x => x.GameID.Equals(game.Id)).Select(y => y.Genre).ToList();
                    result.Add(view);
                }

                return result;
            }

            return null;
        }

        public async Task<GameViewModel> GetGameByIdAsync(int gameId)
        {
            Game game = await _gameRepository.GetByIdAsync(gameId);

            if (game == null)
            {
                return null;
            }

            GameViewModel view = new GameViewModel();
            view.Id = game.Id;
            view.Name = game.Name;
            view.Description = game.Description;
            view.Price = game.Price;
            view.Quantity = game.Quantity;
            view.ImageURL = game.ImageURL;

            return view;
        }

        public async Task<GameDetailsViewModel> GetGameDetailsByIdAsync(int gameId)
        {
            Game game = await _gameRepository.GetByIdAsync(gameId);

            GameDetailsViewModel view = new GameDetailsViewModel();
            view.Id = game.Id;
            view.Name = game.Name;
            view.Description = game.Description;
            view.Price = game.Price;
            view.Quantity = game.Quantity;
            view.ImageURL = game.ImageURL;

            Company company = await _companyRepository.GetByIdAsync(game.CompanyID);
            view.Company = company;


            List<Review> reviews = (await _reviewRepository.GetAllAsync()).Where(g => g.GameID.Equals(gameId)).ToList();
            for (int i = 0; i < reviews.Count; i++)
            {
                reviews[i].CreatedBy = (await _userManager.FindByIdAsync(reviews[i].CreatedById));
            }

            view.Reviews = reviews;

            view.Genres = _context.GameGenres.Where(x => x.GameID.Equals(game.Id)).Select(y => y.Genre).ToList();

            return view;
        }

        //NOT WORKING
        public async Task<GameViewModel> UpdateAsync(GameViewModel gameView, string userId)
        {

            Game game = new Game();
            game.Id = gameView.Id;
            game.Name = gameView.Name;
            game.Description = gameView.Description;
            game.CompanyID = gameView.CompanyID;
            game.Price = gameView.Price;
            game.Quantity = gameView.Quantity;
            game.Created = gameView.Created;
            game.CreatedById = (await _gameRepository.GetByIdAsync(gameView.Id)).CreatedById;
            game.Modified = DateTime.Now;
            game.ModifiedById = userId;

            //removes current corresponding genres
            if (_context.GameGenres.Where(x => x.GameID.Equals(gameView.Id)).Count() > 0)
            {
                foreach (var pair in _context.GameGenres.Where(x => x.GameID.Equals(gameView.Id)).AsQueryable())
                {
                    _context.GameGenres.Remove(pair);
                }
            }

            await _context.SaveChangesAsync();

            //adds new genres to the game
            if (gameView.GenreIds.Count > 0)
            {
                foreach (var genreId in gameView.GenreIds)
                {
                    _context.GameGenres.Add(new GameGenres()
                    {
                        GenreID = genreId,
                        GameID = gameView.Id
                    });

                }
            }
            await _context.SaveChangesAsync();

            await _gameRepository.UpdateAsync(game);

            return gameView;
        }
    }
}
