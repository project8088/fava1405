using Microsoft.EntityFrameworkCore.Migrations;

namespace Nikan.DataLayer.Migrations
{
    public partial class manzalatbimari : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Chk_Bazneshasteh",
                table: "Manzalat");

            migrationBuilder.DropColumn(
                name: "Chk_Janbazan",
                table: "Manzalat");

            migrationBuilder.DropColumn(
                name: "Chk_Maloulin",
                table: "Manzalat");

            migrationBuilder.DropColumn(
                name: "Chk_Salmand",
                table: "Manzalat");

            migrationBuilder.DropColumn(
                name: "Chk_ZananSarparast",
                table: "Manzalat");

            migrationBuilder.AddColumn<int>(
                name: "ManzalatBaseFormId",
                table: "Manzalat",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Typ_SpecialDiseases",
                table: "Manzalat",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Manzalat_ManzalatBaseFormId",
                table: "Manzalat",
                column: "ManzalatBaseFormId");

            migrationBuilder.AddForeignKey(
                name: "FK_Manzalat_ManzalatBaseForm_ManzalatBaseFormId",
                table: "Manzalat",
                column: "ManzalatBaseFormId",
                principalTable: "ManzalatBaseForm",
                principalColumn: "FormId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Manzalat_ManzalatBaseForm_ManzalatBaseFormId",
                table: "Manzalat");

            migrationBuilder.DropIndex(
                name: "IX_Manzalat_ManzalatBaseFormId",
                table: "Manzalat");

            migrationBuilder.DropColumn(
                name: "ManzalatBaseFormId",
                table: "Manzalat");

            migrationBuilder.DropColumn(
                name: "Typ_SpecialDiseases",
                table: "Manzalat");

            migrationBuilder.AddColumn<bool>(
                name: "Chk_Bazneshasteh",
                table: "Manzalat",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Chk_Janbazan",
                table: "Manzalat",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Chk_Maloulin",
                table: "Manzalat",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Chk_Salmand",
                table: "Manzalat",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Chk_ZananSarparast",
                table: "Manzalat",
                type: "bit",
                nullable: true);
        }
    }
}
