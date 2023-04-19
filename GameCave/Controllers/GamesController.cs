using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GameCave.Data;
using Microsoft.AspNetCore.Authorization;
using GameCave.Models.ViewModels.GameVMs;
using Microsoft.AspNetCore.Identity;
using GameCave.Services;

namespace GameCave.Controllers
{
    public class GamesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IWebHostEnvironment _env;
        private readonly IGameService _gameService;

        public GamesController(ApplicationDbContext context, UserManager<IdentityUser> userManager, IWebHostEnvironment env, IGameService gameService)
        {
            _env = env;
            _context = context;
            _userManager = userManager;
            _gameService = gameService;
        }

        [Authorize]
        public async Task<IActionResult> AddToCart(int gameId, string page)
        {
            string userId = (await _userManager.FindByEmailAsync(User.Identity.Name)).Id;
            await EnsureUserHasCart.Ensure(_context, _userManager, userId);

            Game game = await _context.Game.FindAsync(gameId);
            int currentUserCartId = _context.Carts.Where(u => u.OwnerId.Equals(userId)).FirstOrDefault().Id;

            if (game.Quantity > 0)
            {
                _context.CartItems.Add(
                        new CartItem()
                        {
                            GameId = game.Id,
                            Quantity = 1,
                            CartId = currentUserCartId
                        });

                game.Quantity -= 1;
                _context.Game.Update(game);
            }

            await _context.SaveChangesAsync();

            //remains in teh same page but refreshes it
            if (page.Equals("details"))
                return RedirectToAction("Details", "Games", new { id = gameId });

            return RedirectToAction("Index", "Games");
        }


        // GET: Games
        // USES SERVICE
        public async Task<IActionResult> Index()
        {
            var items = _gameService.GetAllGamesAsync();
            if (await items == null || (await items).Count() < 0)
            {
                return View();
            }
            return View(await items);
        }

        // GET: Games/Details/5
        // USES SERVICE
        public async Task<IActionResult> Details(int id)
        {
            if (id == null || _context.Game == null)
            {
                return NotFound();
            }


            string userID = "nouserid";
            bool isAdmin = false;
            if (User.Identity.Name != null)
            {
                userID = (await _userManager.FindByNameAsync(User.Identity.Name)).Id;
                isAdmin = await _userManager.IsInRoleAsync(await _userManager.FindByIdAsync(userID), "Admin");
            }



            ViewData["UserId"] = userID;
            ViewData["IsAdmin"] = isAdmin;
            return View(await _gameService.GetGameDetailsByIdAsync(id));
        }

        // GET: Games/Create
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            ViewData["CompanyID"] = new SelectList(_context.Set<Company>(), "Id", "Name");
            ViewData["GenreID"] = new SelectList(_context.Set<Genre>(), "Id", "Name");
            return View();
        }

        // POST: Games/Create
        // USES SERVICE
        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([FromForm] GameViewModel gameView, IFormFile? imageFile)
        {
            if (ModelState.IsValid)
            {

                //Retrieves the image from the http request and saves it in the root folder
                if (imageFile != null && imageFile.Length > 0)
                {
                    //concatinates the date and time of the upload in order to avoid duplicated image names
                    var fileNameWithExtension = Path.GetFileName(imageFile.FileName);
                    var fileName = fileNameWithExtension.Split('.')[0].Trim() + DateTime.Now.ToString("yyyMMddHHmmssff");
                    var extension = fileNameWithExtension.Split('.')[1].Trim();
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "image", String.Concat(fileName, '.', extension));

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await imageFile.CopyToAsync(stream);
                    }

                    gameView.ImageURL = "/image/" + String.Concat(fileName, '.', extension);
                }

                string userId = (await _userManager.FindByNameAsync(User.Identity.Name)).Id;
                await _gameService.CreateAsync(gameView, userId);
                return RedirectToAction(nameof(Index));
            }

            //var errors = ModelState.Values.SelectMany(v => v.Errors);
            ViewData["CompanyID"] = new SelectList(_context.Set<Company>(), "Id", "Id", gameView.CompanyID);
            ViewData["GenreID"] = new SelectList(_context.Set<Genre>(), "Id", "Id", gameView.CompanyID);
            return View(gameView);
        }

        // GET: Games/Edit/5
        // USES SERVICE
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Game == null)
            {
                return NotFound();
            }

            var gameView = await _gameService.GetGameByIdAsync(id.Value);
            ViewData["CompanyID"] = new SelectList(_context.Set<Company>(), "Id", "Name", gameView.CompanyID);
            ViewData["GenreID"] = new SelectList(_context.Set<Genre>(), "Id", "Name");

            if (gameView == null)
            {
                return NotFound();
            }

            return View(gameView);
        }

        // POST: Games/Edit/5
        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [FromForm] GameViewModel gameView, IFormFile? imageFile)
        {
            if (id != gameView.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Maps the GameViewModel to an entity of type Game
                    Game game = new Game();
                    game.Id = gameView.Id;
                    game.Name = gameView.Name;
                    game.Description = gameView.Description;
                    game.CompanyID = gameView.CompanyID;
                    game.Price = gameView.Price;
                    game.Quantity = gameView.Quantity;
                    game.Created = gameView.Created;
                    game.CreatedById = (await _userManager.FindByNameAsync(User.Identity.Name)).Id;
                    game.Modified = DateTime.Now;
                    game.ModifiedById = (await _userManager.FindByNameAsync(User.Identity.Name)).Id;

                    // Checks if the image of the game has been changed
                    if (imageFile != null)
                    {
                        // Deletes the old image from the root folder
                        string oldImageUrl = gameView.ImageURL;

                        if (!string.IsNullOrEmpty(oldImageUrl))
                        {
                            string url = oldImageUrl.Split('/', StringSplitOptions.RemoveEmptyEntries)[1].Trim();

                            var imageFullPath = string.Concat(Directory.GetCurrentDirectory(), "\\wwwroot\\", "image\\", url);
                            if (System.IO.File.Exists(imageFullPath))
                            {
                                System.IO.File.Delete(imageFullPath);
                            }
                        }

                        var fileNameWithExtension = Path.GetFileName(imageFile.FileName);
                        var fileName = fileNameWithExtension.Split('.')[0].Trim() + DateTime.Now.ToString("yyyMMddHHmmssff");
                        var extension = fileNameWithExtension.Split('.')[1].Trim();
                        var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "image", String.Concat(fileName, '.', extension));

                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await imageFile.CopyToAsync(stream);
                        }

                        game.ImageURL = "/image/" + String.Concat(fileName, '.', extension);
                        gameView.ImageURL = game.ImageURL;
                    }
                    else
                    {
                        game.ImageURL = gameView.ImageURL;
                    }

                    _context.Update(game);

                    // Removes the genres for the current game from table GameGenres
                    if ((await _context.GameGenres.Where(x => x.GameID.Equals(game.Id)).ToListAsync()).Count > 0)
                    {
                        foreach (var pair in await _context.GameGenres.Where(x => x.GameID.Equals(game.Id)).ToListAsync())
                        {
                            _context.Remove(pair);
                            await _context.SaveChangesAsync();
                        }
                    }

                    // Adds the new genres for the current game in table GameGenres
                    if (gameView.GenreIds.Count > 0)
                        foreach (var genreId in gameView.GenreIds)
                        {
                            _context.GameGenres.Add(new GameGenres()
                            {
                                GameID = game.Id,
                                GenreID = genreId
                            });
                        }

                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GameExists(gameView.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            var errors = ModelState.Values.SelectMany(v => v.Errors);

            ViewData["CompanyID"] = new SelectList(_context.Set<Company>(), "Id", "Id", gameView.CompanyID);
            ViewData["GenreID"] = new SelectList(_context.Set<Genre>(), "Id", "Id", gameView.GenreIds);

            return View(gameView);
        }

        // GET: Games/Delete/5
        // USES SERVICE
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Game == null)
            {
                return NotFound();
            }

            var game = await _gameService.GetGameDetailsByIdAsync(id.Value);

            if (game == null)
            {
                return NotFound();
            }

            return View(game);
        }

        // POST: Games/Delete/5
        // USES SERVICE
        [HttpPost, ActionName("Delete")]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Game == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Game'  is null.");
            }

            await _gameService.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }

        private bool GameExists(int id)
        {
            return (_context.Game?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}