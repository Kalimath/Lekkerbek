using Microsoft.EntityFrameworkCore.Migrations;

namespace Lekkerbek.Web.Migrations
{
    public partial class removedTijdslotenFromContext : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bestellingen_Tijdsloten_TijdslotId",
                table: "Bestellingen");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Tijdsloten",
                table: "Tijdsloten");

            migrationBuilder.RenameTable(
                name: "Tijdsloten",
                newName: "Tijdslot");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Tijdslot",
                table: "Tijdslot",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Bestellingen_Tijdslot_TijdslotId",
                table: "Bestellingen",
                column: "TijdslotId",
                principalTable: "Tijdslot",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bestellingen_Tijdslot_TijdslotId",
                table: "Bestellingen");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Tijdslot",
                table: "Tijdslot");

            migrationBuilder.RenameTable(
                name: "Tijdslot",
                newName: "Tijdsloten");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Tijdsloten",
                table: "Tijdsloten",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Bestellingen_Tijdsloten_TijdslotId",
                table: "Bestellingen",
                column: "TijdslotId",
                principalTable: "Tijdsloten",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
