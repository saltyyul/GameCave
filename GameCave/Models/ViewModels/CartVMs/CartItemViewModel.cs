namespace GameCave.Models.ViewModels.CartVMs
{
    public class CartItemViewModel
    {
        public int Id { get; set; }
        public string? ImageUrl { get; set; }
        public string? Name { get; set; }
        public int Quantity { get; set; }
        public double Price { get; set; }
    }
}
