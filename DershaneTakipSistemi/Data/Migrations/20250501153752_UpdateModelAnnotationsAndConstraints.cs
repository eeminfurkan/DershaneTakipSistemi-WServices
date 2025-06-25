using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DershaneTakipSistemi.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateModelAnnotationsAndConstraints : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "TCKimlik",
                table: "Ogrenciler",
                type: "nvarchar(11)",
                maxLength: 11,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Odemeler_OgrenciId",
                table: "Odemeler",
                column: "OgrenciId");

            migrationBuilder.AddForeignKey(
                name: "FK_Odemeler_Ogrenciler_OgrenciId",
                table: "Odemeler",
                column: "OgrenciId",
                principalTable: "Ogrenciler",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Odemeler_Ogrenciler_OgrenciId",
                table: "Odemeler");

            migrationBuilder.DropIndex(
                name: "IX_Odemeler_OgrenciId",
                table: "Odemeler");

            migrationBuilder.AlterColumn<string>(
                name: "TCKimlik",
                table: "Ogrenciler",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(11)",
                oldMaxLength: 11,
                oldNullable: true);
        }
    }
}
