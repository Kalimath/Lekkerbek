using Microsoft.EntityFrameworkCore.Migrations;

namespace Lekkerbek.Web.Migrations
{
    public partial class bestellingKlantIdRefactorToKlantNaam : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bestellingen_Klanten_KlantNaam",
                table: "Bestellingen");

            migrationBuilder.DropForeignKey(
                name: "FK_Gerecht_Bestellingen_BestellingId",
                table: "Gerecht");

            migrationBuilder.DropForeignKey(
                name: "FK_Gerecht_Klanten_KlantNaam",
                table: "Gerecht");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Gerecht",
                table: "Gerecht");

            migrationBuilder.DropIndex(
                name: "IX_Gerecht_BestellingId",
                table: "Gerecht");

            migrationBuilder.DropIndex(
                name: "IX_Bestellingen_KlantNaam",
                table: "Bestellingen");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "Gerecht");

            migrationBuilder.DropColumn(
                name: "BestellingId",
                table: "Gerecht");

            migrationBuilder.DropColumn(
                name: "Omschrijving",
                table: "Gerecht");

            migrationBuilder.RenameColumn(
                name: "KlantNaam",
                table: "Gerecht",
                newName: "KlantId");

            migrationBuilder.RenameIndex(
                name: "IX_Gerecht_KlantNaam",
                table: "Gerecht",
                newName: "IX_Gerecht_KlantId");

            migrationBuilder.AddColumn<string>(
                name: "Naam",
                table: "Gerecht",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<string>(
                name: "KlantNaam",
                table: "Bestellingen",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "KlantId",
                table: "Bestellingen",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Opmerkingen",
                table: "Bestellingen",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Gerecht",
                table: "Gerecht",
                column: "Naam");

            migrationBuilder.CreateTable(
                name: "BestellingGerecht",
                columns: table => new
                {
                    BestellingenId = table.Column<int>(type: "int", nullable: false),
                    GerechtenLijstNaam = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BestellingGerecht", x => new { x.BestellingenId, x.GerechtenLijstNaam });
                    table.ForeignKey(
                        name: "FK_BestellingGerecht_Bestellingen_BestellingenId",
                        column: x => x.BestellingenId,
                        principalTable: "Bestellingen",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BestellingGerecht_Gerecht_GerechtenLijstNaam",
                        column: x => x.GerechtenLijstNaam,
                        principalTable: "Gerecht",
                        principalColumn: "Naam",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Bestellingen_KlantId",
                table: "Bestellingen",
                column: "KlantId");

            migrationBuilder.CreateIndex(
                name: "IX_BestellingGerecht_GerechtenLijstNaam",
                table: "BestellingGerecht",
                column: "GerechtenLijstNaam");

            migrationBuilder.AddForeignKey(
                name: "FK_Bestellingen_Klanten_KlantId",
                table: "Bestellingen",
                column: "KlantId",
                principalTable: "Klanten",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Gerecht_Klanten_KlantId",
                table: "Gerecht",
                column: "KlantId",
                principalTable: "Klanten",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bestellingen_Klanten_KlantId",
                table: "Bestellingen");

            migrationBuilder.DropForeignKey(
                name: "FK_Gerecht_Klanten_KlantId",
                table: "Gerecht");

            migrationBuilder.DropTable(
                name: "BestellingGerecht");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Gerecht",
                table: "Gerecht");

            migrationBuilder.DropIndex(
                name: "IX_Bestellingen_KlantId",
                table: "Bestellingen");

            migrationBuilder.DropColumn(
                name: "Naam",
                table: "Gerecht");

            migrationBuilder.DropColumn(
                name: "KlantId",
                table: "Bestellingen");

            migrationBuilder.DropColumn(
                name: "Opmerkingen",
                table: "Bestellingen");

            migrationBuilder.RenameColumn(
                name: "KlantId",
                table: "Gerecht",
                newName: "KlantNaam");

            migrationBuilder.RenameIndex(
                name: "IX_Gerecht_KlantId",
                table: "Gerecht",
                newName: "IX_Gerecht_KlantNaam");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "Gerecht",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<int>(
                name: "BestellingId",
                table: "Gerecht",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Omschrijving",
                table: "Gerecht",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "KlantNaam",
                table: "Bestellingen",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Gerecht",
                table: "Gerecht",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Gerecht_BestellingId",
                table: "Gerecht",
                column: "BestellingId");

            migrationBuilder.CreateIndex(
                name: "IX_Bestellingen_KlantNaam",
                table: "Bestellingen",
                column: "KlantNaam");

            migrationBuilder.AddForeignKey(
                name: "FK_Bestellingen_Klanten_KlantNaam",
                table: "Bestellingen",
                column: "KlantNaam",
                principalTable: "Klanten",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Gerecht_Bestellingen_BestellingId",
                table: "Gerecht",
                column: "BestellingId",
                principalTable: "Bestellingen",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Gerecht_Klanten_KlantNaam",
                table: "Gerecht",
                column: "KlantNaam",
                principalTable: "Klanten",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
