using Microsoft.EntityFrameworkCore.Migrations;

namespace Nikan.DataLayer.Migrations
{
    public partial class changemanzalatState : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BazneshastehDenyDesc",
                table: "Manzalat");

            migrationBuilder.DropColumn(
                name: "BazneshastehResult",
                table: "Manzalat");

            migrationBuilder.DropColumn(
                name: "JanbazanDenyDesc",
                table: "Manzalat");

            migrationBuilder.DropColumn(
                name: "JanbazanResult",
                table: "Manzalat");

            migrationBuilder.DropColumn(
                name: "MaloulinDenyDesc",
                table: "Manzalat");

            migrationBuilder.DropColumn(
                name: "MaloulinResult",
                table: "Manzalat");

            migrationBuilder.DropColumn(
                name: "SalmandDenyDesc",
                table: "Manzalat");

            migrationBuilder.DropColumn(
                name: "SalmandResult",
                table: "Manzalat");

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

            migrationBuilder.DropColumn(
                name: "ZananSarparastDenyDesc",
                table: "Manzalat");

            migrationBuilder.DropColumn(
                name: "ZananSarparastResult",
                table: "Manzalat");

            migrationBuilder.AddColumn<string>(
                name: "DenyDescription",
                table: "Manzalat",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DenyDescription",
                table: "Manzalat");

            migrationBuilder.AddColumn<string>(
                name: "BazneshastehDenyDesc",
                table: "Manzalat",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "BazneshastehResult",
                table: "Manzalat",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "JanbazanDenyDesc",
                table: "Manzalat",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "JanbazanResult",
                table: "Manzalat",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MaloulinDenyDesc",
                table: "Manzalat",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "MaloulinResult",
                table: "Manzalat",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SalmandDenyDesc",
                table: "Manzalat",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "SalmandResult",
                table: "Manzalat",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SpecialDiseasesDenyDesc",
                table: "Manzalat",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "SpecialDiseasesResult",
                table: "Manzalat",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "WomenWithThreeChildrenDenyDesc",
                table: "Manzalat",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "WomenWithThreeChildrenResult",
                table: "Manzalat",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ZananSarparastDenyDesc",
                table: "Manzalat",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "ZananSarparastResult",
                table: "Manzalat",
                type: "bit",
                nullable: true);
        }
    }
}
