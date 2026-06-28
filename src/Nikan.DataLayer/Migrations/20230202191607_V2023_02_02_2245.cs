using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Nikan.DataLayer.Migrations
{
    public partial class V2023_02_02_2245 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ArchiveSmsInfo",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    MessageText = table.Column<string>(nullable: true),
                    Mobiles = table.Column<string>(nullable: true),
                    GroupListId = table.Column<long>(nullable: false),
                    MessageId = table.Column<long>(nullable: false),
                    SmsStatus = table.Column<int>(nullable: false),
                    StatusText = table.Column<string>(nullable: true),
                    Sender = table.Column<string>(nullable: true),
                    SendOnDate = table.Column<DateTime>(nullable: false),
                    Date = table.Column<long>(nullable: false),
                    Cost = table.Column<int>(nullable: false),
                    Token20 = table.Column<string>(nullable: true),
                    Token10 = table.Column<string>(nullable: true),
                    Token3 = table.Column<string>(nullable: true),
                    Token2 = table.Column<string>(nullable: true),
                    Token1 = table.Column<string>(nullable: true),
                    TempleteName = table.Column<string>(nullable: true),
                    UserId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArchiveSmsInfo", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ArchiveSmsInfo_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ArchiveSmsInfo_UserId",
                table: "ArchiveSmsInfo",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ArchiveSmsInfo");
        }
    }
}
