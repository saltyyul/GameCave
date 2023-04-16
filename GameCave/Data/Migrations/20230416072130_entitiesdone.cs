using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GameCave.Data.Migrations
{
    public partial class entitiesdone : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Carts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OwnerId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Carts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Carts_AspNetUsers_OwnerId",
                        column: x => x.OwnerId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Company",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Modified = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedById = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    ModifiedById = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Company", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Company_AspNetUsers_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Company_AspNetUsers_ModifiedById",
                        column: x => x.ModifiedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Genre",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Modified = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedById = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    ModifiedById = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Genre", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Genre_AspNetUsers_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Genre_AspNetUsers_ModifiedById",
                        column: x => x.ModifiedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Game",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Price = table.Column<double>(type: "float", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    CompanyID = table.Column<int>(type: "int", nullable: false),
                    ImageURL = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Modified = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedById = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    ModifiedById = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Game", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Game_AspNetUsers_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Game_AspNetUsers_ModifiedById",
                        column: x => x.ModifiedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Game_Company_CompanyID",
                        column: x => x.CompanyID,
                        principalTable: "Company",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CartItems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GameId = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    CartId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CartItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CartItems_Carts_CartId",
                        column: x => x.CartId,
                        principalTable: "Carts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CartItems_Game_GameId",
                        column: x => x.GameId,
                        principalTable: "Game",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GameGenres",
                columns: table => new
                {
                    GenreID = table.Column<int>(type: "int", nullable: false),
                    GameID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GameGenres", x => new { x.GenreID, x.GameID });
                    table.ForeignKey(
                        name: "FK_GameGenres_Game_GameID",
                        column: x => x.GameID,
                        principalTable: "Game",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GameGenres_Genre_GenreID",
                        column: x => x.GenreID,
                        principalTable: "Genre",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Review",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    GameID = table.Column<int>(type: "int", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Modified = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedById = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    ModifiedById = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Review", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Review_AspNetUsers_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Review_AspNetUsers_ModifiedById",
                        column: x => x.ModifiedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Review_Game_GameID",
                        column: x => x.GameID,
                        principalTable: "Game",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Company",
                columns: new[] { "Id", "Created", "CreatedById", "Modified", "ModifiedById", "Name" },
                values: new object[,]
                {
                    { 1, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Ubisoft" },
                    { 2, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Electronic arts" },
                    { 3, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Nintendo" },
                    { 4, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Epic games" },
                    { 5, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Gameloft" },
                    { 6, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Sony IE" }
                });

            migrationBuilder.InsertData(
                table: "Genre",
                columns: new[] { "Id", "Created", "CreatedById", "Modified", "ModifiedById", "Name" },
                values: new object[,]
                {
                    { 1, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Horror" },
                    { 2, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Action" },
                    { 3, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Third Person Shooter" },
                    { 4, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Adventure" },
                    { 5, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Puzzle" },
                    { 6, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "2D" },
                    { 7, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Open world" },
                    { 8, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Racing" },
                    { 9, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Fighting" },
                    { 10, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Shooter" },
                    { 11, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "First person shooter" },
                    { 12, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Peaceful" },
                    { 13, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Cooking" },
                    { 14, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Strategic" },
                    { 15, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Co-op" }
                });

            migrationBuilder.InsertData(
                table: "Game",
                columns: new[] { "Id", "CompanyID", "Created", "CreatedById", "Description", "ImageURL", "Modified", "ModifiedById", "Name", "Price", "Quantity" },
                values: new object[,]
                {
                    { 1, 1, new DateTime(2023, 4, 16, 10, 21, 29, 934, DateTimeKind.Local).AddTicks(8820), null, "A game about survival indeed", "https://m.media-amazon.com/images/M/MV5BZGUzYTI3M2EtZmM0Yy00NGUyLWI4ODEtN2Q3ZGJlYzhhZjU3XkEyXkFqcGdeQXVyNTM0OTY1OQ@@._V1_.jpg", new DateTime(2023, 4, 16, 10, 21, 29, 934, DateTimeKind.Local).AddTicks(8862), null, "The last of us", 50.0, 100 },
                    { 2, 5, new DateTime(2023, 4, 16, 10, 21, 29, 934, DateTimeKind.Local).AddTicks(8865), null, "Lots of chests with banichki <3. Shooting pew pew :))))))))))))) (grumnete me)", "https://pbs.twimg.com/media/Euv50WgXEAMXEMN.jpg", new DateTime(2023, 4, 16, 10, 21, 29, 934, DateTimeKind.Local).AddTicks(8867), null, "CS Go", 150.0, 10 },
                    { 3, 6, new DateTime(2023, 4, 16, 10, 21, 29, 934, DateTimeKind.Local).AddTicks(8869), null, "Fighting creatures. Le le le le. The Witcher 3: Wild Hunt is an action role-playing game with a third-person perspective. Players control Geralt of Rivia, a monster slayer known as a Witcher. Geralt walks, runs, rolls and dodges, and (for the first time in the series) jumps, climbs and swims.", "https://w0.peakpx.com/wallpaper/109/423/HD-wallpaper-the-witcher-3-wild-hunt-logo-iphone-7-6s-6-plus-and-pixel-xl-one-plus-3-3t-5-games-and-background-thumbnail.jpg", new DateTime(2023, 4, 16, 10, 21, 29, 934, DateTimeKind.Local).AddTicks(8871), null, "The Witcher", 30.0, 20 },
                    { 4, 3, new DateTime(2023, 4, 16, 10, 21, 29, 934, DateTimeKind.Local).AddTicks(8873), null, "Just shoot zombies, duuh! Resident Evil 4 is a 2023 survival horror game developed and published by Capcom. It is a remake of the 2005 game Resident Evil 4. Players control the US agent Leon S. Kennedy, who must save Ashley Graham, the daughter of the United States president, from the mysterious Los Iluminados cult.", "https://image.api.playstation.com/vulcan/ap/rnd/202210/0712/Excr73ZmEiSIQT18r070yhX5.png", new DateTime(2023, 4, 16, 10, 21, 29, 934, DateTimeKind.Local).AddTicks(8875), null, "Resident Evil 4 Remake Deluxe", 99999.0, 1 },
                    { 5, 2, new DateTime(2023, 4, 16, 10, 21, 29, 934, DateTimeKind.Local).AddTicks(8877), null, "The best game for every single fifth grader on the planet earth! In Minecraft, players explore a blocky, procedurally generated, three-dimensional world with virtually infinite terrain and may discover and extract raw materials, craft tools and items, and build structures, earthworks, and machines.", "https://upload.wikimedia.org/wikipedia/en/5/51/Minecraft_cover.png", new DateTime(2023, 4, 16, 10, 21, 29, 934, DateTimeKind.Local).AddTicks(8878), null, "Minecraft", 10.0, 100 },
                    { 6, 4, new DateTime(2023, 4, 16, 10, 21, 29, 934, DateTimeKind.Local).AddTicks(8881), null, "BANICHKIIIIIIII! Overcooked is a chaotic couch co-op cooking game for one to four players. Working as a team, you and your fellow chefs must prepare, cook and serve up a variety of tasty orders before the baying customers storm out in a huff.", "https://image.api.playstation.com/vulcan/img/rnd/202011/2700/hKBAhyUmI0fv5JrDxITp5rRJ.png", new DateTime(2023, 4, 16, 10, 21, 29, 934, DateTimeKind.Local).AddTicks(8882), null, "Overcooked", 19.989999999999998, 200 },
                    { 7, 5, new DateTime(2023, 4, 16, 10, 21, 29, 934, DateTimeKind.Local).AddTicks(8885), null, "Apex Legends is a non-free-to-play (meaning that you need to pay!) battle royale-hero shooter game developed by Respawn Entertainment and published by Electronic Arts. It was released for PlayStation 4, Windows, and Xbox One in February 2019, for Nintendo Switch in March 2021, and for PlayStation 5 and Xbox Series X/S in March 2022.", "https://upload.wikimedia.org/wikipedia/en/d/db/Apex_legends_cover.jpg", new DateTime(2023, 4, 16, 10, 21, 29, 934, DateTimeKind.Local).AddTicks(8886), null, "Apex Legends", 109.98999999999999, 20 },
                    { 8, 1, new DateTime(2023, 4, 16, 10, 21, 29, 934, DateTimeKind.Local).AddTicks(8888), null, "THE OLD LEAGUE OF LEGENDS! Dota 2 is a 2013 multiplayer online battle arena video game by Valve. The game is a sequel to Defense of the Ancients, a community-created mod for Blizzard Entertainment's Warcraft III: Reign of Chaos. ", "https://pbs.twimg.com/profile_images/1478893871199186945/1mA6tezL_400x400.jpg", new DateTime(2023, 4, 16, 10, 21, 29, 934, DateTimeKind.Local).AddTicks(8889), null, "Dota 2", 9.9900000000000002, 20 }
                });

            migrationBuilder.InsertData(
                table: "GameGenres",
                columns: new[] { "GameID", "GenreID" },
                values: new object[,]
                {
                    { 1, 1 },
                    { 4, 1 },
                    { 5, 1 },
                    { 1, 2 },
                    { 2, 2 },
                    { 3, 2 },
                    { 4, 2 },
                    { 7, 2 },
                    { 1, 3 },
                    { 4, 3 },
                    { 7, 3 },
                    { 1, 4 },
                    { 3, 4 },
                    { 5, 4 },
                    { 4, 5 },
                    { 5, 6 },
                    { 1, 7 },
                    { 3, 7 },
                    { 5, 7 },
                    { 8, 8 },
                    { 3, 9 },
                    { 8, 9 },
                    { 2, 10 },
                    { 7, 10 },
                    { 2, 11 },
                    { 7, 11 },
                    { 6, 12 },
                    { 6, 13 },
                    { 6, 14 },
                    { 8, 14 },
                    { 6, 15 },
                    { 8, 15 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_CartItems_CartId",
                table: "CartItems",
                column: "CartId");

            migrationBuilder.CreateIndex(
                name: "IX_CartItems_GameId",
                table: "CartItems",
                column: "GameId");

            migrationBuilder.CreateIndex(
                name: "IX_Carts_OwnerId",
                table: "Carts",
                column: "OwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_Company_CreatedById",
                table: "Company",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Company_ModifiedById",
                table: "Company",
                column: "ModifiedById");

            migrationBuilder.CreateIndex(
                name: "IX_Game_CompanyID",
                table: "Game",
                column: "CompanyID");

            migrationBuilder.CreateIndex(
                name: "IX_Game_CreatedById",
                table: "Game",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Game_ModifiedById",
                table: "Game",
                column: "ModifiedById");

            migrationBuilder.CreateIndex(
                name: "IX_GameGenres_GameID",
                table: "GameGenres",
                column: "GameID");

            migrationBuilder.CreateIndex(
                name: "IX_Genre_CreatedById",
                table: "Genre",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Genre_ModifiedById",
                table: "Genre",
                column: "ModifiedById");

            migrationBuilder.CreateIndex(
                name: "IX_Review_CreatedById",
                table: "Review",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Review_GameID",
                table: "Review",
                column: "GameID");

            migrationBuilder.CreateIndex(
                name: "IX_Review_ModifiedById",
                table: "Review",
                column: "ModifiedById");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CartItems");

            migrationBuilder.DropTable(
                name: "GameGenres");

            migrationBuilder.DropTable(
                name: "Review");

            migrationBuilder.DropTable(
                name: "Carts");

            migrationBuilder.DropTable(
                name: "Genre");

            migrationBuilder.DropTable(
                name: "Game");

            migrationBuilder.DropTable(
                name: "Company");
        }
    }
}
