using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DershaneTakipSistemi.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddKasaHareketiAndRemoveOdemeModules : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Odemeler");

            migrationBuilder.CreateTable(
                name: "KasaHareketleri",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Aciklama = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Tarih = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Tutar = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    HareketYonu = table.Column<int>(type: "int", nullable: false),
                    Kategori = table.Column<int>(type: "int", nullable: false),
                    OdemeYontemi = table.Column<int>(type: "int", nullable: true),
                    OgrenciId = table.Column<int>(type: "int", nullable: true),
                    PersonelId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KasaHareketleri", x => x.Id);
                    table.ForeignKey(
                        name: "FK_KasaHareketleri_Ogrenciler_OgrenciId",
                        column: x => x.OgrenciId,
                        principalTable: "Ogrenciler",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_KasaHareketleri_Personeller_PersonelId",
                        column: x => x.PersonelId,
                        principalTable: "Personeller",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_KasaHareketleri_OgrenciId",
                table: "KasaHareketleri",
                column: "OgrenciId");

            migrationBuilder.CreateIndex(
                name: "IX_KasaHareketleri_PersonelId",
                table: "KasaHareketleri",
                column: "PersonelId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "KasaHareketleri");

            migrationBuilder.CreateTable(
                name: "Odemeler",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OgrenciId = table.Column<int>(type: "int", nullable: false),
                    OdemeTarihi = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Tutar = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Odemeler", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Odemeler_Ogrenciler_OgrenciId",
                        column: x => x.OgrenciId,
                        principalTable: "Ogrenciler",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Odemeler_OgrenciId",
                table: "Odemeler",
                column: "OgrenciId");
        }
    }
}
