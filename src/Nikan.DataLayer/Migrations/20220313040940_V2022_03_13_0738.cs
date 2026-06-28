using Microsoft.EntityFrameworkCore.Migrations;

namespace Nikan.DataLayer.Migrations
{
    public partial class V2022_03_13_0738 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "PostalPercentOutCity",
                table: "CardInfo_Discount",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "PostalPercentInCity",
                table: "CardInfo_Discount",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "DiscountPercent",
                table: "CardInfo_Discount",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)",
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "PostalPercentOutCity",
                table: "CardInfo_Discount",
                type: "decimal(18,2)",
                nullable: true,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "PostalPercentInCity",
                table: "CardInfo_Discount",
                type: "decimal(18,2)",
                nullable: true,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "DiscountPercent",
                table: "CardInfo_Discount",
                type: "decimal(18,2)",
                nullable: true,
                oldClrType: typeof(int),
                oldNullable: true);
        }
    }
}
