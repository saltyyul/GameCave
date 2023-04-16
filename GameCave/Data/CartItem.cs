using System.ComponentModel.DataAnnotations;

namespace GameCave.Data
{
    public class CartItem
    {
        [Key]
        public int Id { get; set; }
        public int GameId { get; set; }
        public Game? Game { get; set; }
        public int Quantity { get; set; }
        public int CartId { get; set; }
    }
}
