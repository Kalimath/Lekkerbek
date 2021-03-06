using Microsoft.EntityFrameworkCore.Migrations;

namespace Lekkerbek.Web.Migrations
{
    public partial class BestellingVoorGerechtenHashSet2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Gerecht",
                table: "Gerecht");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "Gerecht");

            migrationBuilder.AlterColumn<string>(
                name: "Naam",
                table: "Gerecht",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Gerecht",
                table: "Gerecht",
                column: "Naam");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Gerecht",
                table: "Gerecht");

            migrationBuilder.AlterColumn<string>(
                name: "Naam",
                table: "Gerecht",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "Gerecht",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Gerecht",
                table: "Gerecht",
                column: "Id");
        }
    }
}
