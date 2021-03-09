using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Lekkerbek.Web.Migrations
{
    public partial class BestellingZonderTijdslotKlasse : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bestellingen_Tijdsloten_TijdslotId",
                table: "Bestellingen");

            migrationBuilder.DropIndex(
                name: "IX_Bestellingen_TijdslotId",
                table: "Bestellingen");

            migrationBuilder.DropColumn(
                name: "TijdslotId",
                table: "Bestellingen");

            migrationBuilder.DropColumn(
                name: "Tijdstip",
                table: "Bestellingen");

            migrationBuilder.AddColumn<DateTime>(
                name: "Tijdslot",
                table: "Bestellingen",
                type: "datetime2",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Tijdslot",
                table: "Bestellingen");

            migrationBuilder.AddColumn<int>(
                name: "TijdslotId",
                table: "Bestellingen",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "Tijdstip",
                table: "Bestellingen",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.CreateIndex(
                name: "IX_Bestellingen_TijdslotId",
                table: "Bestellingen",
                column: "TijdslotId");

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
