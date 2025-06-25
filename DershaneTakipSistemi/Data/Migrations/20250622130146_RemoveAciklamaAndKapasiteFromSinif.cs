using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DershaneTakipSistemi.Data.Migrations
{
    /// <inheritdoc />
    public partial class RemoveAciklamaAndKapasiteFromSinif : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Aciklama",
                table: "Siniflar");

            migrationBuilder.DropColumn(
                name: "Kapasite",
                table: "Siniflar");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Aciklama",
                table: "Siniflar",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Kapasite",
                table: "Siniflar",
                type: "int",
                nullable: true);
        }
    }
}
