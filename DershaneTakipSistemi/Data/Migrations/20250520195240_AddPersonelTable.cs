using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DershaneTakipSistemi.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddPersonelTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Personeller",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Ad = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Soyad = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    TCKimlik = table.Column<string>(type: "nvarchar(11)", maxLength: 11, nullable: true),
                    Gorevi = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    CepTelefonu = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    IseBaslamaTarihi = table.Column<DateTime>(type: "datetime2", nullable: true),
                    AktifMi = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Personeller", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Personeller");
        }
    }
}
