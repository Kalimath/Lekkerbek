using Microsoft.EntityFrameworkCore.Migrations;

namespace Lekkerbek.Web.Migrations
{
    public partial class removedAgenda : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tijdslot_Agenda_AgendaId",
                table: "Tijdslot");

            migrationBuilder.DropTable(
                name: "Agenda");

            migrationBuilder.DropIndex(
                name: "IX_Tijdslot_AgendaId",
                table: "Tijdslot");

            migrationBuilder.DropColumn(
                name: "AgendaId",
                table: "Tijdslot");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
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
        }
    }
}
