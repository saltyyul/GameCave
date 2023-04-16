namespace GameCave.Data
{
    public class Genre:BaseEntity
    {
        public Genre()
        {
            GameGenres = new HashSet<GameGenres>();
        }
        public string Name { get; set; }
        public virtual ICollection<GameGenres> GameGenres { get; set; }
    }
}
