using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HoulailYassine.Migrations
{
    /// <inheritdoc />
    public partial class AddIsAdminToPersonne : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsAdmin",
                table: "Livreur",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsAdmin",
                table: "Client",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsAdmin",
                table: "Livreur");

            migrationBuilder.DropColumn(
                name: "IsAdmin",
                table: "Client");
        }
    }
}

