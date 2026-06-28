using Microsoft.EntityFrameworkCore.Migrations;

namespace Nikan.DataLayer.Migrations
{
    public partial class V2022_03_23_1904 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "LastUpdateByUserId",
                table: "CardInfo_Discount",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_CardInfo_Discount_LastUpdateByUserId",
                table: "CardInfo_Discount",
                column: "LastUpdateByUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_CardInfo_Discount_Users_LastUpdateByUserId",
                table: "CardInfo_Discount",
                column: "LastUpdateByUserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CardInfo_Discount_Users_LastUpdateByUserId",
                table: "CardInfo_Discount");

            migrationBuilder.DropIndex(
                name: "IX_CardInfo_Discount_LastUpdateByUserId",
                table: "CardInfo_Discount");

            migrationBuilder.DropColumn(
                name: "LastUpdateByUserId",
                table: "CardInfo_Discount");
        }
    }
}
