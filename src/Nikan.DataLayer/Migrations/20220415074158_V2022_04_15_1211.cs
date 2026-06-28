using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Nikan.DataLayer.Migrations
{
    public partial class V2022_04_15_1211 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "UserCode",
                table: "Users",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserCode",
                table: "Users");
        }
    }
}
