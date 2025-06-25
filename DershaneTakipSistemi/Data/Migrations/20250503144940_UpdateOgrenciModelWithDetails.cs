using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DershaneTakipSistemi.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateOgrenciModelWithDetails : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AdSoyad",
                table: "Ogrenciler");

            migrationBuilder.AddColumn<string>(
                name: "Ad",
                table: "Ogrenciler",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Adres",
                table: "Ogrenciler",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "AktifMi",
                table: "Ogrenciler",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "CepTelefonu",
                table: "Ogrenciler",
                type: "nvarchar(15)",
                maxLength: 15,
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
                name: "Soyad",
                table: "Ogrenciler",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Ad",
                table: "Ogrenciler");

            migrationBuilder.DropColumn(
                name: "Adres",
                table: "Ogrenciler");

            migrationBuilder.DropColumn(
                name: "AktifMi",
                table: "Ogrenciler");

            migrationBuilder.DropColumn(
                name: "CepTelefonu",
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
                name: "Soyad",
                table: "Ogrenciler");

            migrationBuilder.AddColumn<string>(
                name: "AdSoyad",
                table: "Ogrenciler",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
