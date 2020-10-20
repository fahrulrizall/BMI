using Microsoft.EntityFrameworkCore.Migrations;

namespace BMI.Migrations
{
    public partial class addrelations : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Fg_Master_data_Masterdatamodelsap_code",
                table: "Fg");

            migrationBuilder.RenameColumn(
                name: "Masterdatamodelsap_code",
                table: "Fg",
                newName: "masterdatamodelsap_code");

            migrationBuilder.RenameIndex(
                name: "IX_Fg_Masterdatamodelsap_code",
                table: "Fg",
                newName: "IX_Fg_masterdatamodelsap_code");

            migrationBuilder.AddForeignKey(
                name: "FK_Fg_Master_data_masterdatamodelsap_code",
                table: "Fg",
                column: "masterdatamodelsap_code",
                principalTable: "Master_data",
                principalColumn: "sap_code",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Fg_Master_data_masterdatamodelsap_code",
                table: "Fg");

            migrationBuilder.RenameColumn(
                name: "masterdatamodelsap_code",
                table: "Fg",
                newName: "Masterdatamodelsap_code");

            migrationBuilder.RenameIndex(
                name: "IX_Fg_masterdatamodelsap_code",
                table: "Fg",
                newName: "IX_Fg_Masterdatamodelsap_code");

            migrationBuilder.AddForeignKey(
                name: "FK_Fg_Master_data_Masterdatamodelsap_code",
                table: "Fg",
                column: "Masterdatamodelsap_code",
                principalTable: "Master_data",
                principalColumn: "sap_code",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
