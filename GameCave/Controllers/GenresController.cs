using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GameCave.Data;
using Microsoft.AspNetCore.Authorization;
using GameCave.Services;
using Microsoft.AspNetCore.Identity;

namespace GameCave.Controllers
{
    public class GenresController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IGenreService _genreService;
        private readonly UserManager<IdentityUser> _userManager;
        public GenresController(ApplicationDbContext context, IGenreService genreService, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
            _genreService = genreService;
        }

        // GET: Genres
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Index()
        {
            var item = _genreService.GetAllGenresAsync();
            return View(await item);
        }

        // GET: Genres/Details/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Genre == null)
            {
                return NotFound();
            }

            var genre = await _genreService.GetGenreByIdAsync(id.Value);

            if (genre == null)
            {
                return NotFound();
            }

            return View(genre);
        }

        // GET: Genres/Create
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Genres/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,Id")] Genre genre)
        {
            if (ModelState.IsValid)
            {
                string userId = (await _userManager.FindByNameAsync(User.Identity.Name)).Id;
                await _genreService.CreateAsync(genre, userId);
                return RedirectToAction(nameof(Index));
            }
            return View(genre);
        }

        // GET: Genres/Edit/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Genre == null)
            {
                return NotFound();
            }

            var genre = await _genreService.GetGenreByIdAsync(id.Value);

            if (genre == null)
            {
                return NotFound();
            }
            return View(genre);
        }

        // POST: Genres/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Name,Id")] Genre genre)
        {
            if (id != genre.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    string userId = (await _userManager.FindByNameAsync(User.Identity.Name)).Id;
                    await _genreService.UpdateAsync(genre, userId);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GenreExists(genre.Id))
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
            return View(genre);
        }

        // GET: Genres/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Genre == null)
            {
                return NotFound();
            }

            var genre = await _genreService.GetGenreByIdAsync(id.Value);
            if (genre == null)
            {
                return NotFound();
            }

            return View(genre);
        }

        // POST: Genres/Delete/5
        [HttpPost, ActionName("Delete")]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Genre == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Genre'  is null.");
            }

            await _genreService.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }

        private bool GenreExists(int id)
        {
            return (_context.Genre?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}