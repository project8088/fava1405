using Microsoft.EntityFrameworkCore.Migrations;

namespace Nikan.DataLayer.Migrations
{
    public partial class V2022_03_19_1759 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "GroupId",
                table: "ExportCitizens",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ExportCitizens_GroupId",
                table: "ExportCitizens",
                column: "GroupId");

            migrationBuilder.AddForeignKey(
                name: "FK_ExportCitizens_Group_GroupId",
                table: "ExportCitizens",
                column: "GroupId",
                principalTable: "Group",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ExportCitizens_Group_GroupId",
                table: "ExportCitizens");

            migrationBuilder.DropIndex(
                name: "IX_ExportCitizens_GroupId",
                table: "ExportCitizens");

            migrationBuilder.DropColumn(
                name: "GroupId",
                table: "ExportCitizens");
        }
    }
}
