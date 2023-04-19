using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GameCave.Data;
using GameCave.Models.ViewModels.ReviewVMs;
using Microsoft.AspNetCore.Identity;
using GameCave.Services;
using Microsoft.AspNetCore.Authorization;

namespace GameCave.Controllers
{
    public class ReviewsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private IReviewService _reviewService;

        public ReviewsController(ApplicationDbContext context, UserManager<IdentityUser> userManager, IReviewService reviewService)
        {
            _context = context;
            _userManager = userManager;
            _reviewService = reviewService;
        }

        // GET: Reviews
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Index()
        {
            var items = _reviewService.GetAllReviewsAsync();
            return View(await items);
        }

        // GET: Reviews/Details/5

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Review == null)
            {
                return NotFound();
            }

            return View(await _reviewService.GetReviewByIdAsync(id.Value));
        }

        // GET: Reviews/Create

        [Authorize]
        public IActionResult Create(int gameId)
        {
            ViewData["gameId"] = gameId;
            return View();
        }

        // POST: Reviews/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([FromForm] ReviewViewModel reviewView)
        {
            if (ModelState.IsValid)
            {
                string userId = (await _userManager.FindByNameAsync(User.Identity.Name)).Id;
                await _reviewService.CreateAsync(reviewView, userId);
                return RedirectToAction("Details", "Games", new { id = reviewView.GameID });
            }
            var errors = ModelState.Values.SelectMany(v => v.Errors);
            return View(reviewView);
        }

        // GET: Reviews/Edit/5
        [Authorize]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Review == null)
            {
                return NotFound();
            }

            var review = await _reviewService.GetReviewByIdAsync(id.Value);

            if (review == null)
            {
                return NotFound();
            }

            return View(review);
        }

        // POST: Reviews/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [FromForm] ReviewViewModel reviewView)
        {
            if (id != reviewView.Id)
            {
                return NotFound();
            }

#pragma warning disable CS8602 // Dereference of a possibly null reference.
            reviewView.GameID = (await _context.Review.FindAsync(id)).GameID;


            if (ModelState.IsValid)
            {
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
                Review review = await _context.Review.FindAsync(id);
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.
                review.Title = reviewView.Title;
                review.Description = reviewView.Description;
                review.GameID = reviewView.GameID;
                review.Modified = DateTime.Now;
                review.ModifiedById = (await _userManager.FindByNameAsync(User.Identity.Name)).Id;
                review.ModifiedBy = await _userManager.FindByNameAsync(User.Identity.Name);

                _context.Update(review);
                await _context.SaveChangesAsync();

                //string userId = (await _userManager.FindByNameAsync(User.Identity.Name)).Id;
                //await _reviewService.UpdateAsync(reviewView, userId);

                return RedirectToAction("Details", "Games", new { id = reviewView.GameID });
            }
#pragma warning restore CS8602 // Dereference of a possibly null reference.
            var errors = ModelState.Values.SelectMany(v => v.Errors);

            ViewData["CreatedById"] = new SelectList(_context.Users, "Id", "Id", reviewView.CreatedById);
            ViewData["GameID"] = new SelectList(_context.Game, "Id", "Id", reviewView.GameID);
            ViewData["ModifiedById"] = new SelectList(_context.Users, "Id", "Id", reviewView.ModifiedById);

            return View(reviewView);
        }

        // GET: Reviews/Delete/5
        [Authorize]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Review == null)
            {
                return NotFound();
            }

            var review = await _reviewService.GetReviewByIdAsync(id.Value);

            if (review == null)
            {
                return NotFound();
            }

            return View(review);
        }

        // POST: Reviews/Delete/5
        [HttpPost, ActionName("Delete")]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Review == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Review'  is null.");
            }

            int gameId = (await _reviewService.GetReviewByIdAsync(id)).GameID;
            await _reviewService.DeleteAsync(id);

            return RedirectToAction("Details", "Games", new { id = gameId });
        }
    }
}