using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DershaneTakipSistemi.Data.Migrations
{
    /// <inheritdoc />
    public partial class SimplifyPersonelModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CepTelefonu",
                table: "Personeller");

            migrationBuilder.DropColumn(
                name: "Email",
                table: "Personeller");

            migrationBuilder.DropColumn(
                name: "IseBaslamaTarihi",
                table: "Personeller");

            migrationBuilder.DropColumn(
                name: "TCKimlik",
                table: "Personeller");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CepTelefonu",
                table: "Personeller",
                type: "nvarchar(15)",
                maxLength: 15,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "Personeller",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "IseBaslamaTarihi",
                table: "Personeller",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TCKimlik",
                table: "Personeller",
                type: "nvarchar(11)",
                maxLength: 11,
                nullable: true);
        }
    }
}
