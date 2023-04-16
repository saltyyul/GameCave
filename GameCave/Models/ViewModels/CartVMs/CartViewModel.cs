using Microsoft.AspNetCore.Identity;

namespace GameCave.Models.ViewModels.CartVMs
{
    public class CartViewModel
    {
        public int Id { get; set; }
        public List<CartItemViewModel>? Items { get; set; }
        public string? OwnerId { get; set; }
        public IdentityUser? Owner { get; set; }
    }
}
