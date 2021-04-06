using Microsoft.EntityFrameworkCore.Migrations;

namespace Lekkerbek.Web.Migrations
{
    public partial class addedForeignKeyToTijdslot : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "KokId",
                table: "Tijdslot",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Tijdslot_KokId",
                table: "Tijdslot",
                column: "KokId");

            migrationBuilder.AddForeignKey(
                name: "FK_Tijdslot_Koks_KokId",
                table: "Tijdslot",
                column: "KokId",
                principalTable: "Koks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tijdslot_Koks_KokId",
                table: "Tijdslot");

            migrationBuilder.DropIndex(
                name: "IX_Tijdslot_KokId",
                table: "Tijdslot");

            migrationBuilder.DropColumn(
                name: "KokId",
                table: "Tijdslot");
        }
    }
}
