using Microsoft.EntityFrameworkCore.Migrations;

namespace Nikan.DataLayer.Migrations
{
    public partial class V2023_02_04_1104 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OwnerId",
                table: "ManzalatDocumentInfo");

            migrationBuilder.AddColumn<int>(
                name: "Count",
                table: "ExportedCitizens",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Count",
                table: "ExportedCitizens");

            migrationBuilder.AddColumn<int>(
                name: "OwnerId",
                table: "ManzalatDocumentInfo",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
