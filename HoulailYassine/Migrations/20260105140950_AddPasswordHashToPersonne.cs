using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HoulailYassine.Migrations
{
    /// <inheritdoc />
    public partial class AddPasswordHashToPersonne : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PasswordHash",
                table: "Livreur",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PasswordHash",
                table: "Client",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PasswordHash",
                table: "Livreur");

            migrationBuilder.DropColumn(
                name: "PasswordHash",
                table: "Client");
        }
    }
}

