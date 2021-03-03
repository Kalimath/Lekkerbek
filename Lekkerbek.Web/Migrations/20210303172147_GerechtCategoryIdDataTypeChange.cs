using Microsoft.EntityFrameworkCore.Migrations;

namespace Lekkerbek.Web.Migrations
{
    public partial class GerechtCategoryIdDataTypeChange : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Gerecht_Categorie_CategorieNaam",
                table: "Gerecht");

            migrationBuilder.DropIndex(
                name: "IX_Gerecht_CategorieNaam",
                table: "Gerecht");

            migrationBuilder.DropColumn(
                name: "CategorieNaam",
                table: "Gerecht");

            migrationBuilder.AlterColumn<string>(
                name: "CategorieId",
                table: "Gerecht",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateIndex(
                name: "IX_Gerecht_CategorieId",
                table: "Gerecht",
                column: "CategorieId");

            migrationBuilder.AddForeignKey(
                name: "FK_Gerecht_Categorie_CategorieId",
                table: "Gerecht",
                column: "CategorieId",
                principalTable: "Categorie",
                principalColumn: "Naam",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Gerecht_Categorie_CategorieId",
                table: "Gerecht");

            migrationBuilder.DropIndex(
                name: "IX_Gerecht_CategorieId",
                table: "Gerecht");

            migrationBuilder.AlterColumn<int>(
                name: "CategorieId",
                table: "Gerecht",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CategorieNaam",
                table: "Gerecht",
                type: "nvarchar(450)",
                nullable: true);

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
    }
}
