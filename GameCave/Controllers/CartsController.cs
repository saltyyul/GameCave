using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GameCave.Data;
using Microsoft.AspNetCore.Identity;
using GameCave.Models.ViewModels.CartVMs;
using GameCave.Services;
using GameCave.Models.ViewModels.GameVMs;
using Microsoft.AspNetCore.Authorization;

namespace GameCave.Controllers
{
    public class CartsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IGameService _gameService;

        public CartsController(ApplicationDbContext context, UserManager<IdentityUser> userManager, IGameService gameService)
        {
            _context = context;
            _userManager = userManager;
            _gameService = gameService;
        }

        [Authorize]
        public async Task<IActionResult> Index()
        {
            string userId = (await _userManager.FindByNameAsync(User.Identity.Name)).Id;

            await EnsureUserHasCart.Ensure(_context, _userManager, userId);

            var cart = _context.Carts.Include(x => x.Items).Where(c => c.OwnerId.Equals(userId)).FirstOrDefault();

            //maps the cart to cartviewmodel
            var cartViewModel = new CartViewModel();
            cartViewModel.Id = cart.Id;
            cartViewModel.Owner = cart.Owner;
            cartViewModel.OwnerId = cart.OwnerId;
            cartViewModel.Items = new List<CartItemViewModel>();

            double total = 0.0;
            foreach (var item in cart.Items)
            {
                GameViewModel game = await _gameService.GetGameByIdAsync(item.GameId);

                //map from cartitem to cartitemviewmodel
                CartItemViewModel cartItemViewModel = new CartItemViewModel()
                {
                    Id = item.Id,
                    ImageUrl = game.ImageURL,
                    Name = game.Name,
                    Quantity = item.Quantity,
                    Price = game.Price
                };
                cartViewModel.Items.Add(cartItemViewModel);
                total += cartItemViewModel.Quantity * cartItemViewModel.Price;
            }

            ViewBag.Total = total;

            return View(cartViewModel);
        }

        public async Task<IActionResult> IncreaseQuantity(int cartItemId)
        {
            var item = await _context.CartItems.FindAsync(cartItemId);
            var game = await _context.Game.FindAsync(item.GameId);

            if (game.Quantity - 1 >= 0)
            {
                item.Quantity += 1;

                DecreaseGameQuantityFromDbWithOne(game);

                _context.CartItems.Update(item);
                await _context.SaveChangesAsync();

                CartItemViewModel cartItemViewModel = new CartItemViewModel()
                {
                    Id = item.Id,
                    ImageUrl = game.ImageURL,
                    Name = game.Name,
                    Quantity = item.Quantity
                };
            }

            return RedirectToAction("Index", "Carts");
        }

        public async Task<IActionResult> DecreaseQuantity(int cartItemId)
        {
            var item = await _context.CartItems.FindAsync(cartItemId);

            Game game = await _context.Game.FindAsync(item.GameId);

            if (item.Quantity - 1 >= 0)
                item.Quantity -= 1;

            //if is 0 remove from cart
            if (item.Quantity == 0)
            {
                _context.CartItems.Remove(item);
                await _context.SaveChangesAsync();
            }
            else
            {
                _context.CartItems.Update(item);
                await _context.SaveChangesAsync();

                CartItemViewModel cartItemViewModel = new CartItemViewModel()
                {
                    Id = item.Id,
                    ImageUrl = game.ImageURL,
                    Name = game.Name,
                    Quantity = item.Quantity
                };
            }

            IncreaseGameQuantityInDbWithOne(game);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index", "Carts");

        }

        public async Task<IActionResult> RemoveGame(int cartItemId)
        {
            await this.RemoveGameByCartItemId(cartItemId);
            return RedirectToAction("Index", "Carts");
        }


        public async Task<IActionResult> RemoveAll()
        {
            var userId = (await _userManager.FindByNameAsync(User.Identity.Name)).Id;
            var usersCartId = _context.Carts.Where(u => u.OwnerId.Equals(userId)).Select(c => c.Id).First();

            foreach (var item in _context.CartItems.Where(c => c.CartId.Equals(usersCartId)))
            {
                await this.RemoveGameByCartItemId(item.Id);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction("Index", "Carts");
        }
        private async Task<bool> RemoveGameByCartItemId(int cartItemId)
        {
            try
            {
                var item = await _context.CartItems.FindAsync(cartItemId);
                var quantity = item.Quantity;
                Game game = await _context.Game.FindAsync(item.GameId);
                game.Quantity += quantity;
                _context.Game.Update(game);
                _context.CartItems.Remove(item);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        private async void DecreaseGameQuantityFromDbWithOne(Game game)
        {
            game.Quantity -= 1;
            _context.Game.Update(game);
            //await _context.SaveChangesAsync();
        }

        private async void IncreaseGameQuantityInDbWithOne(Game game)
        {
            game.Quantity += 1;
            _context.Game.Update(game);
            //await _context.SaveChangesAsync();
        }
    }
}