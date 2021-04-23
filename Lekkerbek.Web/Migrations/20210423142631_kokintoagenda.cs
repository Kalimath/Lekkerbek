using Microsoft.EntityFrameworkCore.Migrations;

namespace Lekkerbek.Web.Migrations
{
    public partial class kokintoagenda : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tijdslot_Koks_KokId",
                table: "Tijdslot");

            migrationBuilder.DropTable(
                name: "Koks");

            migrationBuilder.DropColumn(
                name: "InGebruikDoorPersoneel",
                table: "Bestellingen");

            migrationBuilder.RenameColumn(
                name: "KokId",
                table: "Tijdslot",
                newName: "InGebruikDoorKokId");

            migrationBuilder.RenameIndex(
                name: "IX_Tijdslot_KokId",
                table: "Tijdslot",
                newName: "IX_Tijdslot_InGebruikDoorKokId");

            migrationBuilder.AddColumn<int>(
                name: "AgendaId",
                table: "Tijdslot",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Agenda",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Agenda", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Tijdslot_AgendaId",
                table: "Tijdslot",
                column: "AgendaId");

            migrationBuilder.AddForeignKey(
                name: "FK_Tijdslot_Agenda_AgendaId",
                table: "Tijdslot",
                column: "AgendaId",
                principalTable: "Agenda",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Tijdslot_AspNetUsers_InGebruikDoorKokId",
                table: "Tijdslot",
                column: "InGebruikDoorKokId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tijdslot_Agenda_AgendaId",
                table: "Tijdslot");

            migrationBuilder.DropForeignKey(
                name: "FK_Tijdslot_AspNetUsers_InGebruikDoorKokId",
                table: "Tijdslot");

            migrationBuilder.DropTable(
                name: "Agenda");

            migrationBuilder.DropIndex(
                name: "IX_Tijdslot_AgendaId",
                table: "Tijdslot");

            migrationBuilder.DropColumn(
                name: "AgendaId",
                table: "Tijdslot");

            migrationBuilder.RenameColumn(
                name: "InGebruikDoorKokId",
                table: "Tijdslot",
                newName: "KokId");

            migrationBuilder.RenameIndex(
                name: "IX_Tijdslot_InGebruikDoorKokId",
                table: "Tijdslot",
                newName: "IX_Tijdslot_KokId");

            migrationBuilder.AddColumn<bool>(
                name: "InGebruikDoorPersoneel",
                table: "Bestellingen",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "Koks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Naam = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Koks", x => x.Id);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_Tijdslot_Koks_KokId",
                table: "Tijdslot",
                column: "KokId",
                principalTable: "Koks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
