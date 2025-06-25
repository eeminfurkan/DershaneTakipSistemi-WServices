using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DershaneTakipSistemi.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddOgrenciSinifRelation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SinifId",
                table: "Ogrenciler",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Ogrenciler_SinifId",
                table: "Ogrenciler",
                column: "SinifId");

            migrationBuilder.AddForeignKey(
                name: "FK_Ogrenciler_Siniflar_SinifId",
                table: "Ogrenciler",
                column: "SinifId",
                principalTable: "Siniflar",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Ogrenciler_Siniflar_SinifId",
                table: "Ogrenciler");

            migrationBuilder.DropIndex(
                name: "IX_Ogrenciler_SinifId",
                table: "Ogrenciler");

            migrationBuilder.DropColumn(
                name: "SinifId",
                table: "Ogrenciler");
        }
    }
}
