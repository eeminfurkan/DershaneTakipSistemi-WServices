using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DershaneTakipSistemi.Data.Migrations
{
    /// <inheritdoc />
    public partial class SetRestrictDeleteOnOdemeler : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Odemeler_Ogrenciler_OgrenciId",
                table: "Odemeler");

            migrationBuilder.AddForeignKey(
                name: "FK_Odemeler_Ogrenciler_OgrenciId",
                table: "Odemeler",
                column: "OgrenciId",
                principalTable: "Ogrenciler",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Odemeler_Ogrenciler_OgrenciId",
                table: "Odemeler");

            migrationBuilder.AddForeignKey(
                name: "FK_Odemeler_Ogrenciler_OgrenciId",
                table: "Odemeler",
                column: "OgrenciId",
                principalTable: "Ogrenciler",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
