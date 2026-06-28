using Microsoft.EntityFrameworkCore.Migrations;

namespace Nikan.DataLayer.Migrations
{
    public partial class V2023_12_21_1308 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "GuidId",
                table: "CardInfo_DistributeCard_QueueInfo",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "GuidId",
                table: "CardInfo_DistributeCard_Courses",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "GuidId",
                table: "CardInfo_DistributeCard_QueueInfo");

            migrationBuilder.DropColumn(
                name: "GuidId",
                table: "CardInfo_DistributeCard_Courses");
        }
    }
}
