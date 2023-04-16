namespace GameCave.Data
{
    public class Company:BaseEntity
    {
        public Company()
        {
            Games = new HashSet<Game>();
        }
        public string Name { get; set; }

        public virtual ICollection<Game> Games { get; set; }
    }
}
