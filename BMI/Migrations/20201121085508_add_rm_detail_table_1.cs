using Microsoft.EntityFrameworkCore.Migrations;

namespace BMI.Migrations
{
    public partial class add_rm_detail_table_1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Rm_detail_RmModel_raw_source",
                table: "Rm_detail");

            migrationBuilder.DropPrimaryKey(
                name: "PK_RmModel",
                table: "RmModel");

            migrationBuilder.RenameTable(
                name: "RmModel",
                newName: "Rm");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Rm",
                table: "Rm",
                column: "raw_source");

            migrationBuilder.AddForeignKey(
                name: "FK_Rm_detail_Rm_raw_source",
                table: "Rm_detail",
                column: "raw_source",
                principalTable: "Rm",
                principalColumn: "raw_source",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Rm_detail_Rm_raw_source",
                table: "Rm_detail");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Rm",
                table: "Rm");

            migrationBuilder.RenameTable(
                name: "Rm",
                newName: "RmModel");

            migrationBuilder.AddPrimaryKey(
                name: "PK_RmModel",
                table: "RmModel",
                column: "raw_source");

            migrationBuilder.AddForeignKey(
                name: "FK_Rm_detail_RmModel_raw_source",
                table: "Rm_detail",
                column: "raw_source",
                principalTable: "RmModel",
                principalColumn: "raw_source",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
