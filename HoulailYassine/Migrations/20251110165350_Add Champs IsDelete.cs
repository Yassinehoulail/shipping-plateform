using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HoulailYassine.Migrations
{
    /// <inheritdoc />
    public partial class AddChampsIsDelete : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsDelete",
                table: "Livreurs",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDelete",
                table: "Commandes",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDelete",
                table: "Clients",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsDelete",
                table: "Livreurs");

            migrationBuilder.DropColumn(
                name: "IsDelete",
                table: "Commandes");

            migrationBuilder.DropColumn(
                name: "IsDelete",
                table: "Clients");
        }
    }
}

