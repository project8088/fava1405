using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Nikan.DataLayer.Migrations
{
    public partial class V2023_01_27_1340 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ArchiveEvent",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    CreateDate = table.Column<DateTime>(nullable: false),
                    ActionName = table.Column<string>(nullable: true),
                    EventSection = table.Column<int>(nullable: false),
                    EventPriority = table.Column<int>(nullable: false),
                    EventType = table.Column<int>(nullable: false),
                    Code = table.Column<int>(nullable: false),
                    StrCode = table.Column<string>(nullable: true),
                    OperationId = table.Column<int>(nullable: true),
                    UserId = table.Column<int>(nullable: true),
                    WebSite = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    UserIp = table.Column<string>(nullable: true),
                    JsonValue = table.Column<string>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArchiveEvent", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ArchiveEvent_Users_OperationId",
                        column: x => x.OperationId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ArchiveEvent_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CitizensAuthentication",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CitizenId = table.Column<int>(nullable: false),
                    OnDate = table.Column<DateTime>(nullable: false),
                    AddByUserId = table.Column<int>(nullable: true),
                    SabtStatus = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CitizensAuthentication", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CitizensAuthentication_Users_AddByUserId",
                        column: x => x.AddByUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CitizensAuthentication_Citizen_CitizenId",
                        column: x => x.CitizenId,
                        principalTable: "Citizen",
                        principalColumn: "CitizenId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ArchiveEvent_OperationId",
                table: "ArchiveEvent",
                column: "OperationId");

            migrationBuilder.CreateIndex(
                name: "IX_ArchiveEvent_UserId",
                table: "ArchiveEvent",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_CitizensAuthentication_AddByUserId",
                table: "CitizensAuthentication",
                column: "AddByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_CitizensAuthentication_CitizenId",
                table: "CitizensAuthentication",
                column: "CitizenId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ArchiveEvent");

            migrationBuilder.DropTable(
                name: "CitizensAuthentication");
        }
    }
}
