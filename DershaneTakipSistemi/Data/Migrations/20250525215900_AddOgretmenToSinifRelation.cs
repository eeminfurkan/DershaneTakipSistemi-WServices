using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DershaneTakipSistemi.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddOgretmenToSinifRelation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "OgretmenPersonelId",
                table: "Siniflar",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Siniflar_OgretmenPersonelId",
                table: "Siniflar",
                column: "OgretmenPersonelId");

            migrationBuilder.AddForeignKey(
                name: "FK_Siniflar_Personeller_OgretmenPersonelId",
                table: "Siniflar",
                column: "OgretmenPersonelId",
                principalTable: "Personeller",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Siniflar_Personeller_OgretmenPersonelId",
                table: "Siniflar");

            migrationBuilder.DropIndex(
                name: "IX_Siniflar_OgretmenPersonelId",
                table: "Siniflar");

            migrationBuilder.DropColumn(
                name: "OgretmenPersonelId",
                table: "Siniflar");
        }
    }
}
