using Microsoft.EntityFrameworkCore.Migrations;

namespace Lekkerbek.Web.Migrations
{
    public partial class Voorkeursgerechten : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Voorkeursgerechten",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GerechtId = table.Column<int>(type: "int", nullable: false),
                    GerechtNaam = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    KlantId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Voorkeursgerechten", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Voorkeursgerechten_AspNetUsers_KlantId",
                        column: x => x.KlantId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Voorkeursgerechten_Gerecht_GerechtNaam",
                        column: x => x.GerechtNaam,
                        principalTable: "Gerecht",
                        principalColumn: "Naam",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Voorkeursgerechten_GerechtNaam",
                table: "Voorkeursgerechten",
                column: "GerechtNaam");

            migrationBuilder.CreateIndex(
                name: "IX_Voorkeursgerechten_KlantId",
                table: "Voorkeursgerechten",
                column: "KlantId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Voorkeursgerechten");
        }
    }
}
