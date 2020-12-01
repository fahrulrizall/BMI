using Microsoft.EntityFrameworkCore.Migrations;

namespace BMI.Migrations
{
    public partial class change_table_part8 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Production_input_Master_BMI_bmi_code",
                table: "Production_input");

            migrationBuilder.DropIndex(
                name: "IX_Production_input_bmi_code",
                table: "Production_input");

            migrationBuilder.DropColumn(
                name: "bmi_code",
                table: "Production_input");

            migrationBuilder.AddColumn<string>(
                name: "sap_code",
                table: "Production_input",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Production_input_sap_code",
                table: "Production_input",
                column: "sap_code");

            migrationBuilder.AddForeignKey(
                name: "FK_Production_input_Master_data_sap_code",
                table: "Production_input",
                column: "sap_code",
                principalTable: "Master_data",
                principalColumn: "sap_code",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Production_input_Master_data_sap_code",
                table: "Production_input");

            migrationBuilder.DropIndex(
                name: "IX_Production_input_sap_code",
                table: "Production_input");

            migrationBuilder.DropColumn(
                name: "sap_code",
                table: "Production_input");

            migrationBuilder.AddColumn<string>(
                name: "bmi_code",
                table: "Production_input",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Production_input_bmi_code",
                table: "Production_input",
                column: "bmi_code");

            migrationBuilder.AddForeignKey(
                name: "FK_Production_input_Master_BMI_bmi_code",
                table: "Production_input",
                column: "bmi_code",
                principalTable: "Master_BMI",
                principalColumn: "bmi_code",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
