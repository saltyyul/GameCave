using GameCave.Data;
using Microsoft.AspNetCore.Identity;

namespace GameCave.Controllers
{
    public static class EnsureUserHasCart
    {
        public static async Task<bool> Ensure(ApplicationDbContext context, UserManager<IdentityUser> userManager, string userId)
        {
            try
            {
                bool doesCurrentUserHaveCart = false;

                if (context.Carts.Count() > 0)
                {
                    //If isn't null, user has cart
                    doesCurrentUserHaveCart = context.Carts
                        .Where(c => c.OwnerId.Equals(userId))
                        .FirstOrDefault() != null;

                }

                //Doesn't have cart, create one
                if (!doesCurrentUserHaveCart) 
                {
                    Cart cart = new Cart();
                    cart.OwnerId = userId;
                    cart.Owner = await userManager.FindByIdAsync(userId);
                    cart.Items = new List<CartItem>();
                    context.Carts.Add(cart);
                    await context.SaveChangesAsync();
                }

                //Ensured that user has cart
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}