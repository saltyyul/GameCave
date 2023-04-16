namespace GameCave.Data
{
    public class Game:BaseEntity
    {
        public Game()
        {
            GameGenres = new HashSet<GameGenres>();
        }
        public string Name { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }
        public int Quantity { get; set; }
        public int CompanyID { get; set; }

        //to establish connection with company entity 
        //can be null
        public Company? Company { get; set; }

        //to create conenction with gameGenres
        public ICollection<GameGenres>? GameGenres { get; set; }

        //To establish connection with Reviews
        //can be null as one game might not have any reviews
        public ICollection<Review>? Reviews { get; set; }

        //[NotMapped]
        //[DisplayName("Upload file")]
        //public IFormFile ImageFile { get; set; }

        public string? ImageURL { get; set; }
    }
}
