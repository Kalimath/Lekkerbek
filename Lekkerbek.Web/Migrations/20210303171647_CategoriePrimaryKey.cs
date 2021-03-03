using Microsoft.EntityFrameworkCore.Migrations;

namespace Lekkerbek.Web.Migrations
{
    public partial class CategoriePrimaryKey : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Gerecht_Categorie_CategorieId",
                table: "Gerecht");

            migrationBuilder.DropIndex(
                name: "IX_Gerecht_CategorieId",
                table: "Gerecht");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Categorie",
                table: "Categorie");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "Categorie");

            migrationBuilder.AddColumn<string>(
                name: "CategorieNaam",
                table: "Gerecht",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Naam",
                table: "Categorie",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Categorie",
                table: "Categorie",
                column: "Naam");

            migrationBuilder.CreateIndex(
                name: "IX_Gerecht_CategorieNaam",
                table: "Gerecht",
                column: "CategorieNaam");

            migrationBuilder.AddForeignKey(
                name: "FK_Gerecht_Categorie_CategorieNaam",
                table: "Gerecht",
                column: "CategorieNaam",
                principalTable: "Categorie",
                principalColumn: "Naam",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Gerecht_Categorie_CategorieNaam",
                table: "Gerecht");

            migrationBuilder.DropIndex(
                name: "IX_Gerecht_CategorieNaam",
                table: "Gerecht");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Categorie",
                table: "Categorie");

            migrationBuilder.DropColumn(
                name: "CategorieNaam",
                table: "Gerecht");

            migrationBuilder.AlterColumn<string>(
                name: "Naam",
                table: "Categorie",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "Categorie",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Categorie",
                table: "Categorie",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Gerecht_CategorieId",
                table: "Gerecht",
                column: "CategorieId");

            migrationBuilder.AddForeignKey(
                name: "FK_Gerecht_Categorie_CategorieId",
                table: "Gerecht",
                column: "CategorieId",
                principalTable: "Categorie",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
