using GameCave.Data;

namespace GameCave.Models.ViewModels.GameVMs
{
    public class GameDetailsViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }
        public int Quantity { get; set; }
        public Company? Company { get; set; }
        public ICollection<Review>? Reviews { get; set; }
        public ICollection<Genre>? Genres { get; set; }
        public string? ImageURL { get; set; }
    }
}
