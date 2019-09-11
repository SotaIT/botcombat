using Microsoft.EntityFrameworkCore.Migrations;

namespace BotCombat.Web.Data.Migrations
{
    public partial class TrapRemoveImage : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageId",
                table: "Traps");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ImageId",
                table: "Traps",
                nullable: false,
                defaultValue: 0);
        }
    }
}
