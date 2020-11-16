using Microsoft.EntityFrameworkCore.Migrations;

namespace BMI.Migrations
{
    public partial class change_colum_name_pt_on_table_Destroy : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Destroy_Pt_pt",
                table: "Destroy");

            migrationBuilder.DropIndex(
                name: "IX_Destroy_pt",
                table: "Destroy");

            migrationBuilder.DropColumn(
                name: "pt",
                table: "Destroy");

            migrationBuilder.AddColumn<string>(
                name: "id_pt",
                table: "Destroy",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Destroy_id_pt",
                table: "Destroy",
                column: "id_pt");

            migrationBuilder.AddForeignKey(
                name: "FK_Destroy_Pt_id_pt",
                table: "Destroy",
                column: "id_pt",
                principalTable: "Pt",
                principalColumn: "id_pt",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Destroy_Pt_id_pt",
                table: "Destroy");

            migrationBuilder.DropIndex(
                name: "IX_Destroy_id_pt",
                table: "Destroy");

            migrationBuilder.DropColumn(
                name: "id_pt",
                table: "Destroy");

            migrationBuilder.AddColumn<string>(
                name: "pt",
                table: "Destroy",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Destroy_pt",
                table: "Destroy",
                column: "pt");

            migrationBuilder.AddForeignKey(
                name: "FK_Destroy_Pt_pt",
                table: "Destroy",
                column: "pt",
                principalTable: "Pt",
                principalColumn: "id_pt",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
