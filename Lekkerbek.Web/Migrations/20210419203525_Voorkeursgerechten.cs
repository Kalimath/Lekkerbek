using Microsoft.EntityFrameworkCore.Migrations;

namespace Lekkerbek.Web.Migrations
{
    public partial class Voorkeursgerechten : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Gerecht_AspNetUsers_GebruikerId",
                table: "Gerecht");

            migrationBuilder.DropIndex(
                name: "IX_Gerecht_GebruikerId",
                table: "Gerecht");

            migrationBuilder.DropColumn(
                name: "GebruikerId",
                table: "Gerecht");

            migrationBuilder.CreateTable(
                name: "GebruikerGerecht",
                columns: table => new
                {
                    VoorkeursgerechtenNaam = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    VoorkeursgerechtenVanKlantenId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GebruikerGerecht", x => new { x.VoorkeursgerechtenNaam, x.VoorkeursgerechtenVanKlantenId });
                    table.ForeignKey(
                        name: "FK_GebruikerGerecht_AspNetUsers_VoorkeursgerechtenVanKlantenId",
                        column: x => x.VoorkeursgerechtenVanKlantenId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GebruikerGerecht_Gerecht_VoorkeursgerechtenNaam",
                        column: x => x.VoorkeursgerechtenNaam,
                        principalTable: "Gerecht",
                        principalColumn: "Naam",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GebruikerGerecht_VoorkeursgerechtenVanKlantenId",
                table: "GebruikerGerecht",
                column: "VoorkeursgerechtenVanKlantenId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GebruikerGerecht");

            migrationBuilder.AddColumn<int>(
                name: "GebruikerId",
                table: "Gerecht",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Gerecht_GebruikerId",
                table: "Gerecht",
                column: "GebruikerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Gerecht_AspNetUsers_GebruikerId",
                table: "Gerecht",
                column: "GebruikerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
