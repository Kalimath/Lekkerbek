using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Lekkerbek.Web.Migrations
{
    public partial class AddedKalender2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OpeningsUren");
            migrationBuilder.CreateTable(
                name: "OpeningsUren",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Dag = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Startuur = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SluitingsUur = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OpeningsUren", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "VerlofDagenVanGebruikers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GebruikerId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VerlofDagenVanGebruikers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ZiekteDagenVanGebruikers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GebruikerId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ZiekteDagenVanGebruikers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Dag",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Datum = table.Column<DateTime>(type: "datetime2", nullable: false),
                    VerlofDagenVanGebruikerId = table.Column<int>(type: "int", nullable: true),
                    ZiekteDagenVanGebruikerId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Dag", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Dag_VerlofDagenVanGebruikers_VerlofDagenVanGebruikerId",
                        column: x => x.VerlofDagenVanGebruikerId,
                        principalTable: "VerlofDagenVanGebruikers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Dag_ZiekteDagenVanGebruikers_ZiekteDagenVanGebruikerId",
                        column: x => x.ZiekteDagenVanGebruikerId,
                        principalTable: "ZiekteDagenVanGebruikers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Dag_VerlofDagenVanGebruikerId",
                table: "Dag",
                column: "VerlofDagenVanGebruikerId");

            migrationBuilder.CreateIndex(
                name: "IX_Dag_ZiekteDagenVanGebruikerId",
                table: "Dag",
                column: "ZiekteDagenVanGebruikerId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Dag");

            migrationBuilder.DropTable(
                name: "OpeningsUren");

            migrationBuilder.DropTable(
                name: "VerlofDagenVanGebruikers");

            migrationBuilder.DropTable(
                name: "ZiekteDagenVanGebruikers");
        }
    }
}
