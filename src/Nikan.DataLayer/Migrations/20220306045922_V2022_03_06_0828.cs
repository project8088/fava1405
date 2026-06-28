using Microsoft.EntityFrameworkCore.Migrations;

namespace Nikan.DataLayer.Migrations
{
    public partial class V2022_03_06_0828 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "CenterIsActive",
                table: "CardInfo_Discount_Center",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "CardInfo_Discount",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CenterIsActive",
                table: "CardInfo_Discount_Center");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "CardInfo_Discount");
        }
    }
}
