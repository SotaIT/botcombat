using Microsoft.EntityFrameworkCore.Migrations;

namespace BotCombat.Web.Data.Migrations
{
    public partial class Shots : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ActionTimeout",
                table: "Maps",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "MemoryLimit",
                table: "Maps",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "BulletImageId",
                table: "BotImages",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ExplosionImageId",
                table: "BotImages",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ShotImageId",
                table: "BotImages",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ActionTimeout",
                table: "Maps");

            migrationBuilder.DropColumn(
                name: "MemoryLimit",
                table: "Maps");

            migrationBuilder.DropColumn(
                name: "BulletImageId",
                table: "BotImages");

            migrationBuilder.DropColumn(
                name: "ExplosionImageId",
                table: "BotImages");

            migrationBuilder.DropColumn(
                name: "ShotImageId",
                table: "BotImages");
        }
    }
}
