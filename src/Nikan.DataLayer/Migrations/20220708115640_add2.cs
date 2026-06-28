using Microsoft.EntityFrameworkCore.Migrations;

namespace Nikan.DataLayer.Migrations
{
    public partial class add2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "SpecialDiseasesDenyDesc",
                table: "Manzalat",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "SpecialDiseasesResult",
                table: "Manzalat",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "WomenWithThreeChildrenDenyDesc",
                table: "Manzalat",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "WomenWithThreeChildrenResult",
                table: "Manzalat",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SpecialDiseasesDenyDesc",
                table: "Manzalat");

            migrationBuilder.DropColumn(
                name: "SpecialDiseasesResult",
                table: "Manzalat");

            migrationBuilder.DropColumn(
                name: "WomenWithThreeChildrenDenyDesc",
                table: "Manzalat");

            migrationBuilder.DropColumn(
                name: "WomenWithThreeChildrenResult",
                table: "Manzalat");
        }
    }
}
