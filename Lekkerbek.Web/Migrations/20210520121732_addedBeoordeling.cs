using Microsoft.EntityFrameworkCore.Migrations;

namespace Lekkerbek.Web.Migrations
{
    public partial class addedBeoordeling : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.CreateTable(
                name: "ScoreLijst",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ServiceScore = table.Column<double>(type: "float", nullable: false),
                    EtenEnDrinkenScore = table.Column<double>(type: "float", nullable: false),
                    PrijsKwaliteitScore = table.Column<double>(type: "float", nullable: false),
                    HygieneScore = table.Column<double>(type: "float", nullable: false),
                    BeoordelingsId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ScoreLijst", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Beoordelingen",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TotaalScore = table.Column<int>(type: "int", nullable: false),
                    ScoreLijstId = table.Column<int>(type: "int", nullable: true),
                    Commentaar = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    KlantId = table.Column<int>(type: "int", nullable: false),
                    GebruikerId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Beoordelingen", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Beoordelingen_AspNetUsers_GebruikerId",
                        column: x => x.GebruikerId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Beoordelingen_ScoreLijst_ScoreLijstId",
                        column: x => x.ScoreLijstId,
                        principalTable: "ScoreLijst",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Beoordelingen_GebruikerId",
                table: "Beoordelingen",
                column: "GebruikerId");

            migrationBuilder.CreateIndex(
                name: "IX_Beoordelingen_ScoreLijstId",
                table: "Beoordelingen",
                column: "ScoreLijstId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Beoordelingen");

            migrationBuilder.DropTable(
                name: "ScoreLijst");

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
    }
}
