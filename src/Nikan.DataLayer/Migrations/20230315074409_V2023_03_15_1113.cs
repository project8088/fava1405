using Microsoft.EntityFrameworkCore.Migrations;

namespace Nikan.DataLayer.Migrations
{
    public partial class V2023_03_15_1113 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "note1",
                table: "CitizensAuthentication",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "note2",
                table: "CitizensAuthentication",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "note3",
                table: "CitizensAuthentication",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "note1",
                table: "CitizensAuthentication");

            migrationBuilder.DropColumn(
                name: "note2",
                table: "CitizensAuthentication");

            migrationBuilder.DropColumn(
                name: "note3",
                table: "CitizensAuthentication");
        }
    }
}
