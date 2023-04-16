namespace GameCave.Data
{
    public class Review:BaseEntity
    {
        public string Description { get; set; }
        public string Title { get; set; }
        public int GameID { get; set; }
        public virtual Game? Game { get; set; }
    }
}
