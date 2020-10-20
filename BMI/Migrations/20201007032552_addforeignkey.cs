using Microsoft.EntityFrameworkCore.Migrations;

namespace BMI.Migrations
{
    public partial class addforeignkey : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Masterdatamodelsap_code",
                table: "Fg",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Fg_Masterdatamodelsap_code",
                table: "Fg",
                column: "Masterdatamodelsap_code");

            migrationBuilder.AddForeignKey(
                name: "FK_Fg_Master_data_Masterdatamodelsap_code",
                table: "Fg",
                column: "Masterdatamodelsap_code",
                principalTable: "Master_data",
                principalColumn: "sap_code",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Fg_Master_data_Masterdatamodelsap_code",
                table: "Fg");

            migrationBuilder.DropIndex(
                name: "IX_Fg_Masterdatamodelsap_code",
                table: "Fg");

            migrationBuilder.DropColumn(
                name: "Masterdatamodelsap_code",
                table: "Fg");
        }
    }
}
