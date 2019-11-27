using Microsoft.EntityFrameworkCore.Migrations;

namespace BotCombat.Web.Data.Migrations
{
    public partial class Bullets : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "BonusSpawnInterval",
                table: "Maps",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "BulletSpeed",
                table: "Maps",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "RangedWeight",
                table: "Maps",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BonusSpawnInterval",
                table: "Maps");

            migrationBuilder.DropColumn(
                name: "BulletSpeed",
                table: "Maps");

            migrationBuilder.DropColumn(
                name: "RangedWeight",
                table: "Maps");
        }
    }
}
