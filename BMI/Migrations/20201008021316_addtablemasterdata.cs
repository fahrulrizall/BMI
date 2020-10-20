using Microsoft.EntityFrameworkCore.Migrations;

namespace BMI.Migrations
{
    public partial class addtablemasterdata : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.AlterColumn<string>(
                name: "sap_code",
                table: "Rm",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "sap_code",
                table: "Fg",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateIndex(
                name: "IX_Rm_sap_code",
                table: "Rm",
                column: "sap_code");

            migrationBuilder.CreateIndex(
                name: "IX_Fg_sap_code",
                table: "Fg",
                column: "sap_code");

            migrationBuilder.AddForeignKey(
                name: "FK_Fg_Master_data_sap_code",
                table: "Fg",
                column: "sap_code",
                principalTable: "Master_data",
                principalColumn: "sap_code",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Rm_Master_data_sap_code",
                table: "Rm",
                column: "sap_code",
                principalTable: "Master_data",
                principalColumn: "sap_code",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Fg_Master_data_sap_code",
                table: "Fg");

            migrationBuilder.DropForeignKey(
                name: "FK_Rm_Master_data_sap_code",
                table: "Rm");

            migrationBuilder.DropIndex(
                name: "IX_Rm_sap_code",
                table: "Rm");

            migrationBuilder.DropIndex(
                name: "IX_Fg_sap_code",
                table: "Fg");

            migrationBuilder.AlterColumn<string>(
                name: "sap_code",
                table: "Rm",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "sap_code",
                table: "Fg",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string));

            migrationBuilder.AddColumn<string>(
                name: "Masterdatamodelsap_code",
                table: "Fg",
                type: "nvarchar(450)",
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
    }
}
