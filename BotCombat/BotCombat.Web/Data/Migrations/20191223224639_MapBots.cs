using Microsoft.EntityFrameworkCore.Migrations;

namespace BotCombat.Web.Data.Migrations
{
    public partial class MapBots : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BotId",
                table: "StartPoints");

            migrationBuilder.CreateTable(
                name: "MapBots",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MapId = table.Column<int>(nullable: false),
                    BotId = table.Column<int>(nullable: false),
                    BotImageId = table.Column<int>(nullable: false),
                    StartPointId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MapBots", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MapBots");

            migrationBuilder.AddColumn<int>(
                name: "BotId",
                table: "StartPoints",
                type: "int",
                nullable: true);
        }
    }
}
