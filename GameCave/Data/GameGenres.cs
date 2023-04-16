namespace GameCave.Data
{
    public class GameGenres
    {
        public int GenreID { get; set; }
        public virtual Genre Genre { get; set; }
        public int GameID { get; set; }
        public virtual Game Game { get; set; }
    }
}
