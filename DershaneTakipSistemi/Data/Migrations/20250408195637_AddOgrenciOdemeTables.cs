using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DershaneTakipSistemi.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddOgrenciOdemeTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Odemeler",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Tutar = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    OdemeTarihi = table.Column<DateTime>(type: "datetime2", nullable: false),
                    OgrenciId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Odemeler", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Ogrenciler",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AdSoyad = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TCKimlik = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    KayitTarihi = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ogrenciler", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Odemeler");

            migrationBuilder.DropTable(
                name: "Ogrenciler");
        }
    }
}
