using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DershaneTakipSistemi.Data.Migrations
{
    /// <inheritdoc />
    public partial class SimplifyOgrenciModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Adres",
                table: "Ogrenciler");

            migrationBuilder.DropColumn(
                name: "DogumTarihi",
                table: "Ogrenciler");

            migrationBuilder.DropColumn(
                name: "Email",
                table: "Ogrenciler");

            migrationBuilder.DropColumn(
                name: "Notlar",
                table: "Ogrenciler");

            migrationBuilder.DropColumn(
                name: "TCKimlik",
                table: "Ogrenciler");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Adres",
                table: "Ogrenciler",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DogumTarihi",
                table: "Ogrenciler",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "Ogrenciler",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Notlar",
                table: "Ogrenciler",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TCKimlik",
                table: "Ogrenciler",
                type: "nvarchar(11)",
                maxLength: 11,
                nullable: true);
        }
    }
}
