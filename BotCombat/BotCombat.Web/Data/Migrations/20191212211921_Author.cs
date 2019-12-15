using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BotCombat.Web.Data.Migrations
{
    public partial class Author : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "Authors",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "AuthorBot",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AuthorId = table.Column<int>(nullable: false),
                    BotId = table.Column<int>(nullable: false),
                    ParentId = table.Column<int>(nullable: true),
                    Version = table.Column<int>(nullable: false),
                    Created = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuthorBot", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AuthorBot");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Authors");
        }
    }
}
