using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace GameCave.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Call the base method to configure the default behavior
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<GameGenres>()
                .HasOne(gg => gg.Genre)
                .WithMany(g => g.GameGenres)
                .HasForeignKey(gg => gg.GenreID);

            modelBuilder.Entity<GameGenres>()
                .HasOne(gg => gg.Game)
                .WithMany(g => g.GameGenres)
                .HasForeignKey(gg => gg.GameID);

            modelBuilder.Entity<GameGenres>()
                .HasKey(gg => new { gg.GenreID, gg.GameID });

            modelBuilder.Entity<Genre>().HasData(
                new Data.Genre() { Id = 1, Name = "Horror" },
                new Data.Genre() { Id = 2, Name = "Action" },
                new Data.Genre() { Id = 3, Name = "Third Person Shooter" },
                new Data.Genre() { Id = 4, Name = "Adventure" },
                new Data.Genre() { Id = 5, Name = "Puzzle" },
                new Data.Genre() { Id = 6, Name = "2D" },
                new Data.Genre() { Id = 7, Name = "Open world" },
                new Data.Genre() { Id = 8, Name = "Racing" },
                new Data.Genre() { Id = 9, Name = "Fighting" },
                new Data.Genre() { Id = 10, Name = "Shooter" },
                new Data.Genre() { Id = 11, Name = "First person shooter" },
                new Data.Genre() { Id = 12, Name = "Peaceful" },
                new Data.Genre() { Id = 13, Name = "Cooking" },
                new Data.Genre() { Id = 14, Name = "Strategic" },
                new Data.Genre() { Id = 15, Name = "Co-op" }
                );

            modelBuilder.Entity<Company>().HasData(
                new Data.Company() { Id = 1, Name = "Ubisoft" },
                new Data.Company() { Id = 2, Name = "Electronic arts" },
                new Data.Company() { Id = 3, Name = "Nintendo" },
                new Data.Company() { Id = 4, Name = "Epic games" },
                new Data.Company() { Id = 5, Name = "Gameloft" },
                new Data.Company() { Id = 6, Name = "Sony IE" }
                );


            modelBuilder.Entity<Game>().HasData(
                new Game()
                {
                    Id = 1,
                    Name = "The last of us",
                    Description = "A game about survival indeed",
                    Price = 50.00,
                    Quantity = 100,
                    CompanyID = 1,
                    ImageURL = @"https://m.media-amazon.com/images/M/MV5BZGUzYTI3M2EtZmM0Yy00NGUyLWI4ODEtN2Q3ZGJlYzhhZjU3XkEyXkFqcGdeQXVyNTM0OTY1OQ@@._V1_.jpg",
                    Created = DateTime.Now,
                    Modified = DateTime.Now
                },
                new Game()
                {
                    Id = 2,
                    Name = "CS Go",
                    Description = "Lots of chests with banichki <3. Shooting pew pew :))))))))))))) (grumnete me)",
                    Price = 150.00,
                    Quantity = 10,
                    CompanyID = 5,
                    ImageURL = @"https://pbs.twimg.com/media/Euv50WgXEAMXEMN.jpg",
                    Created = DateTime.Now,
                    Modified = DateTime.Now
                },
                new Game()
                {
                    Id = 3,
                    Name = "The Witcher",
                    Description = "Fighting creatures. Le le le le. The Witcher 3: Wild Hunt is an action role-playing game with a third-person perspective. Players control Geralt of Rivia, a monster slayer known as a Witcher. Geralt walks, runs, rolls and dodges, and (for the first time in the series) jumps, climbs and swims.",
                    Price = 30.00,
                    Quantity = 20,
                    CompanyID = 6,
                    ImageURL = @"https://w0.peakpx.com/wallpaper/109/423/HD-wallpaper-the-witcher-3-wild-hunt-logo-iphone-7-6s-6-plus-and-pixel-xl-one-plus-3-3t-5-games-and-background-thumbnail.jpg",
                    Created = DateTime.Now,
                    Modified = DateTime.Now
                },
                new Game()
                {
                    Id = 4,
                    Name = "Resident Evil 4 Remake Deluxe",
                    Description = "Just shoot zombies, duuh! Resident Evil 4 is a 2023 survival horror game developed and published by Capcom. It is a remake of the 2005 game Resident Evil 4. Players control the US agent Leon S. Kennedy, who must save Ashley Graham, the daughter of the United States president, from the mysterious Los Iluminados cult.",
                    Price = 99999.00,
                    Quantity = 1,
                    CompanyID = 3,
                    ImageURL = @"https://image.api.playstation.com/vulcan/ap/rnd/202210/0712/Excr73ZmEiSIQT18r070yhX5.png",
                    Created = DateTime.Now,
                    Modified = DateTime.Now
                },
                new Game()
                {
                    Id = 5,
                    Name = "Minecraft",
                    Description = "The best game for every single fifth grader on the planet earth! In Minecraft, players explore a blocky, procedurally generated, three-dimensional world with virtually infinite terrain and may discover and extract raw materials, craft tools and items, and build structures, earthworks, and machines.",
                    Price = 10.00,
                    Quantity = 100,
                    CompanyID = 2,
                    ImageURL = @"https://upload.wikimedia.org/wikipedia/en/5/51/Minecraft_cover.png",
                    Created = DateTime.Now,
                    Modified = DateTime.Now
                },
                new Game()
                {
                    Id = 6,
                    Name = "Overcooked",
                    Description = "BANICHKIIIIIIII! Overcooked is a chaotic couch co-op cooking game for one to four players. Working as a team, you and your fellow chefs must prepare, cook and serve up a variety of tasty orders before the baying customers storm out in a huff.",
                    Price = 19.99,
                    Quantity = 200,
                    CompanyID = 4,
                    ImageURL = @"https://image.api.playstation.com/vulcan/img/rnd/202011/2700/hKBAhyUmI0fv5JrDxITp5rRJ.png",
                    Created = DateTime.Now,
                    Modified = DateTime.Now
                },
                new Game()
                {
                    Id = 7,
                    Name = "Apex Legends",
                    Description = "Apex Legends is a non-free-to-play (meaning that you need to pay!) battle royale-hero shooter game developed by Respawn Entertainment and published by Electronic Arts. It was released for PlayStation 4, Windows, and Xbox One in February 2019, for Nintendo Switch in March 2021, and for PlayStation 5 and Xbox Series X/S in March 2022.",
                    Price = 109.99,
                    Quantity = 20,
                    CompanyID = 5,
                    ImageURL = @"https://upload.wikimedia.org/wikipedia/en/d/db/Apex_legends_cover.jpg",
                    Created = DateTime.Now,
                    Modified = DateTime.Now
                },
                new Game()
                {
                    Id = 8,
                    Name = "Dota 2",
                    Description = "THE OLD LEAGUE OF LEGENDS! Dota 2 is a 2013 multiplayer online battle arena video game by Valve. The game is a sequel to Defense of the Ancients, a community-created mod for Blizzard Entertainment's Warcraft III: Reign of Chaos. ",
                    Price = 9.99,
                    Quantity = 20,
                    CompanyID = 1,
                    ImageURL = @"https://pbs.twimg.com/profile_images/1478893871199186945/1mA6tezL_400x400.jpg",
                    Created = DateTime.Now,
                    Modified = DateTime.Now
                }
                );

            modelBuilder.Entity<GameGenres>().HasData(
                new GameGenres() { GameID = 1, GenreID = 1 },
                new GameGenres() { GameID = 1, GenreID = 2 },
                new GameGenres() { GameID = 1, GenreID = 3 },
                new GameGenres() { GameID = 1, GenreID = 4 },
                new GameGenres() { GameID = 1, GenreID = 7 },
                new GameGenres() { GameID = 2, GenreID = 10 },
                new GameGenres() { GameID = 2, GenreID = 11 },
                new GameGenres() { GameID = 2, GenreID = 2 },
                new GameGenres() { GameID = 3, GenreID = 2 },
                new GameGenres() { GameID = 3, GenreID = 4 },
                new GameGenres() { GameID = 3, GenreID = 7 },
                new GameGenres() { GameID = 3, GenreID = 9 },
                new GameGenres() { GameID = 4, GenreID = 1 },
                new GameGenres() { GameID = 4, GenreID = 2 },
                new GameGenres() { GameID = 4, GenreID = 3 },
                new GameGenres() { GameID = 4, GenreID = 5 },
                new GameGenres() { GameID = 5, GenreID = 1 },
                new GameGenres() { GameID = 5, GenreID = 7 },
                new GameGenres() { GameID = 5, GenreID = 4 },
                new GameGenres() { GameID = 5, GenreID = 6 },
                new GameGenres() { GameID = 6, GenreID = 12 },
                new GameGenres() { GameID = 6, GenreID = 14 },
                new GameGenres() { GameID = 6, GenreID = 13 },
                new GameGenres() { GameID = 6, GenreID = 15 },
                new GameGenres() { GameID = 7, GenreID = 11 },
                new GameGenres() { GameID = 7, GenreID = 3 },
                new GameGenres() { GameID = 7, GenreID = 2 },
                new GameGenres() { GameID = 7, GenreID = 10 },
                new GameGenres() { GameID = 8, GenreID = 15 },
                new GameGenres() { GameID = 8, GenreID = 14 },
                new GameGenres() { GameID = 8, GenreID = 9 },
                new GameGenres() { GameID = 8, GenreID = 8 }
                );

        }

        public DbSet<GameCave.Data.Game>? Game { get; set; }
        public DbSet<GameCave.Data.Genre>? Genre { get; set; }
        public DbSet<GameCave.Data.Review>? Review { get; set; }
        public DbSet<GameCave.Data.Company>? Company { get; set; }
        public DbSet<GameCave.Data.GameGenres>? GameGenres { get; set; }
        public DbSet<Cart>? Carts { get; set; }
        public DbSet<CartItem>? CartItems { get; set; }
    }
}