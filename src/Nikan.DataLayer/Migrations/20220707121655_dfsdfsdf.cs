using Microsoft.EntityFrameworkCore.Migrations;

namespace Nikan.DataLayer.Migrations
{
    public partial class dfsdfsdf : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Id",
                table: "ManzalatBaseForm");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "ManzalatBaseForm",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
