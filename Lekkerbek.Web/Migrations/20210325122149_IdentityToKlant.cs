using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Lekkerbek.Web.Migrations
{
    public partial class IdentityToKlant : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AccessFailedCount",
                table: "Klanten",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "ConcurrencyStamp",
                table: "Klanten",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "Klanten",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "EmailConfirmed",
                table: "Klanten",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "LockoutEnabled",
                table: "Klanten",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "LockoutEnd",
                table: "Klanten",
                type: "datetimeoffset",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NormalizedEmail",
                table: "Klanten",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NormalizedUserName",
                table: "Klanten",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PasswordHash",
                table: "Klanten",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PhoneNumber",
                table: "Klanten",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "PhoneNumberConfirmed",
                table: "Klanten",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "SecurityStamp",
                table: "Klanten",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "TwoFactorEnabled",
                table: "Klanten",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "UserName",
                table: "Klanten",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AccessFailedCount",
                table: "Klanten");

            migrationBuilder.DropColumn(
                name: "ConcurrencyStamp",
                table: "Klanten");

            migrationBuilder.DropColumn(
                name: "Email",
                table: "Klanten");

            migrationBuilder.DropColumn(
                name: "EmailConfirmed",
                table: "Klanten");

            migrationBuilder.DropColumn(
                name: "LockoutEnabled",
                table: "Klanten");

            migrationBuilder.DropColumn(
                name: "LockoutEnd",
                table: "Klanten");

            migrationBuilder.DropColumn(
                name: "NormalizedEmail",
                table: "Klanten");

            migrationBuilder.DropColumn(
                name: "NormalizedUserName",
                table: "Klanten");

            migrationBuilder.DropColumn(
                name: "PasswordHash",
                table: "Klanten");

            migrationBuilder.DropColumn(
                name: "PhoneNumber",
                table: "Klanten");

            migrationBuilder.DropColumn(
                name: "PhoneNumberConfirmed",
                table: "Klanten");

            migrationBuilder.DropColumn(
                name: "SecurityStamp",
                table: "Klanten");

            migrationBuilder.DropColumn(
                name: "TwoFactorEnabled",
                table: "Klanten");

            migrationBuilder.DropColumn(
                name: "UserName",
                table: "Klanten");
        }
    }
}
