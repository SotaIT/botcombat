using Microsoft.EntityFrameworkCore.Migrations;

namespace BotCombat.Web.Data.Migrations
{
    public partial class MyBots : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_AuthorBot",
                table: "AuthorBot");

            migrationBuilder.RenameTable(
                name: "AuthorBot",
                newName: "AuthorBots");

            migrationBuilder.AddColumn<int>(
                name: "RootId",
                table: "AuthorBots",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "AuthorBots",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_AuthorBots",
                table: "AuthorBots",
                column: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_AuthorBots",
                table: "AuthorBots");

            migrationBuilder.DropColumn(
                name: "RootId",
                table: "AuthorBots");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "AuthorBots");

            migrationBuilder.RenameTable(
                name: "AuthorBots",
                newName: "AuthorBot");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AuthorBot",
                table: "AuthorBot",
                column: "Id");
        }
    }
}
