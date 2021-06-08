using Microsoft.EntityFrameworkCore.Migrations;

namespace Lekkerbek.Web.Migrations
{
    public partial class KlachtIsAfgehandeldToegevoegd : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsAfgehandeld",
                table: "Klachten",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsAfgehandeld",
                table: "Klachten");
        }
    }
}
