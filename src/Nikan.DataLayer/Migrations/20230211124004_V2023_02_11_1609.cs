using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Nikan.DataLayer.Migrations
{
    public partial class V2023_02_11_1609 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CardInfo_RequestFreeCard",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    DiscountTitle = table.Column<string>(nullable: true),
                    CardTypeId = table.Column<int>(nullable: false),
                    CreationDate = table.Column<DateTime>(nullable: false),
                    CreationById = table.Column<int>(nullable: true),
                    LastUpdateByUserId = table.Column<int>(nullable: true),
                    GroupId = table.Column<int>(nullable: true),
                    DeliverType = table.Column<int>(nullable: false),
                    CenterID = table.Column<string>(nullable: true),
                    ImagerReviewStatusFormFreeCard = table.Column<int>(nullable: false),
                    FreeCardApplicantOrganization = table.Column<string>(nullable: true),
                    LetterNumber = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    AttachmentGroup = table.Column<string>(nullable: true),
                    Accepted = table.Column<bool>(nullable: true),
                    AcceptedById = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CardInfo_RequestFreeCard", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CardInfo_RequestFreeCard_Users_AcceptedById",
                        column: x => x.AcceptedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CardInfo_RequestFreeCard_CardType_CardTypeId",
                        column: x => x.CardTypeId,
                        principalTable: "CardType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CardInfo_RequestFreeCard_OrganizationalUnit_CenterID",
                        column: x => x.CenterID,
                        principalTable: "OrganizationalUnit",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CardInfo_RequestFreeCard_Users_CreationById",
                        column: x => x.CreationById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CardInfo_RequestFreeCard_Group_GroupId",
                        column: x => x.GroupId,
                        principalTable: "Group",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CardInfo_RequestFreeCard_Users_LastUpdateByUserId",
                        column: x => x.LastUpdateByUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CardInfo_RequestFreeCard_Citizens",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    RequestFreeCardId = table.Column<string>(nullable: true),
                    CitizenId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CardInfo_RequestFreeCard_Citizens", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CardInfo_RequestFreeCard_Citizens_Citizen_CitizenId",
                        column: x => x.CitizenId,
                        principalTable: "Citizen",
                        principalColumn: "CitizenId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CardInfo_RequestFreeCard_Citizens_CardInfo_RequestFreeCard_RequestFreeCardId",
                        column: x => x.RequestFreeCardId,
                        principalTable: "CardInfo_RequestFreeCard",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CardInfo_RequestFreeCard_AcceptedById",
                table: "CardInfo_RequestFreeCard",
                column: "AcceptedById");

            migrationBuilder.CreateIndex(
                name: "IX_CardInfo_RequestFreeCard_CardTypeId",
                table: "CardInfo_RequestFreeCard",
                column: "CardTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_CardInfo_RequestFreeCard_CenterID",
                table: "CardInfo_RequestFreeCard",
                column: "CenterID");

            migrationBuilder.CreateIndex(
                name: "IX_CardInfo_RequestFreeCard_CreationById",
                table: "CardInfo_RequestFreeCard",
                column: "CreationById");

            migrationBuilder.CreateIndex(
                name: "IX_CardInfo_RequestFreeCard_GroupId",
                table: "CardInfo_RequestFreeCard",
                column: "GroupId");

            migrationBuilder.CreateIndex(
                name: "IX_CardInfo_RequestFreeCard_LastUpdateByUserId",
                table: "CardInfo_RequestFreeCard",
                column: "LastUpdateByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_CardInfo_RequestFreeCard_Citizens_CitizenId",
                table: "CardInfo_RequestFreeCard_Citizens",
                column: "CitizenId");

            migrationBuilder.CreateIndex(
                name: "IX_CardInfo_RequestFreeCard_Citizens_RequestFreeCardId",
                table: "CardInfo_RequestFreeCard_Citizens",
                column: "RequestFreeCardId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CardInfo_RequestFreeCard_Citizens");

            migrationBuilder.DropTable(
                name: "CardInfo_RequestFreeCard");
        }
    }
}
