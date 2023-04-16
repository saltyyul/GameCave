using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace GameCave.Data
{
    public class Cart
    {
        [Key]
        public int Id { get; set; }
        public List<CartItem>? Items { get; set; }
        public string? OwnerId { get; set; }
        public IdentityUser? Owner { get; set; }
    }
}
