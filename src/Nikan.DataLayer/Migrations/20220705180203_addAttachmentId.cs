using Microsoft.EntityFrameworkCore.Migrations;

namespace Nikan.DataLayer.Migrations
{
    public partial class addAttachmentId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AttachmentId",
                table: "Manzalat",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsPublic",
                table: "Attachment",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AttachmentId",
                table: "Manzalat");

            migrationBuilder.DropColumn(
                name: "IsPublic",
                table: "Attachment");
        }
    }
}
