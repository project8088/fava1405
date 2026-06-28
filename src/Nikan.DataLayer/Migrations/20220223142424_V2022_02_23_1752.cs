using Microsoft.EntityFrameworkCore.Migrations;

namespace Nikan.DataLayer.Migrations
{
    public partial class V2022_02_23_1752 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "RegisterByUserId",
                table: "Users",
                nullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "PostalPercentOutCity",
                table: "CardInfo_Discount",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "PostalPercentInCity",
                table: "CardInfo_Discount",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "DiscountPercent",
                table: "CardInfo_Discount",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_RegisterByUserId",
                table: "Users",
                column: "RegisterByUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Users_RegisterByUserId",
                table: "Users",
                column: "RegisterByUserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_Users_RegisterByUserId",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_RegisterByUserId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "RegisterByUserId",
                table: "Users");

            migrationBuilder.AlterColumn<int>(
                name: "PostalPercentOutCity",
                table: "CardInfo_Discount",
                type: "int",
                nullable: true,
                oldClrType: typeof(decimal),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "PostalPercentInCity",
                table: "CardInfo_Discount",
                type: "int",
                nullable: true,
                oldClrType: typeof(decimal),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "DiscountPercent",
                table: "CardInfo_Discount",
                type: "int",
                nullable: true,
                oldClrType: typeof(decimal),
                oldNullable: true);
        }
    }
}
