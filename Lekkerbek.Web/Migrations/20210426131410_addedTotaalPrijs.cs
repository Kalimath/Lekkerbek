using Microsoft.EntityFrameworkCore.Migrations;

namespace Lekkerbek.Web.Migrations
{
    public partial class addedTotaalPrijs : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "PrijsInclBtw",
                table: "Gerecht",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "TotaalPrijs",
                table: "Bestellingen",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "TotaalPrijsInclBtw",
                table: "Bestellingen",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PrijsInclBtw",
                table: "Gerecht");

            migrationBuilder.DropColumn(
                name: "TotaalPrijs",
                table: "Bestellingen");

            migrationBuilder.DropColumn(
                name: "TotaalPrijsInclBtw",
                table: "Bestellingen");
        }
    }
}
