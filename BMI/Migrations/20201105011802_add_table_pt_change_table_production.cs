using Microsoft.EntityFrameworkCore.Migrations;

namespace BMI.Migrations
{
    public partial class add_table_pt_change_table_production : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Production_output_Master_BMI_bmi_code_repack",
                table: "Production_output");

            migrationBuilder.DropIndex(
                name: "IX_Production_output_bmi_code_repack",
                table: "Production_output");

            migrationBuilder.DropColumn(
                name: "batch",
                table: "Production_output");

            migrationBuilder.DropColumn(
                name: "batch_repack",
                table: "Production_output");

            migrationBuilder.DropColumn(
                name: "bmi_code_repack",
                table: "Production_output");

            migrationBuilder.AddColumn<string>(
                name: "MasterBMIModel1bmi_code",
                table: "Production_output",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Production_output_MasterBMIModel1bmi_code",
                table: "Production_output",
                column: "MasterBMIModel1bmi_code");

            migrationBuilder.AddForeignKey(
                name: "FK_Production_output_Master_BMI_MasterBMIModel1bmi_code",
                table: "Production_output",
                column: "MasterBMIModel1bmi_code",
                principalTable: "Master_BMI",
                principalColumn: "bmi_code",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Production_output_Master_BMI_MasterBMIModel1bmi_code",
                table: "Production_output");

            migrationBuilder.DropIndex(
                name: "IX_Production_output_MasterBMIModel1bmi_code",
                table: "Production_output");

            migrationBuilder.DropColumn(
                name: "MasterBMIModel1bmi_code",
                table: "Production_output");

            migrationBuilder.AddColumn<string>(
                name: "batch",
                table: "Production_output",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "batch_repack",
                table: "Production_output",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "bmi_code_repack",
                table: "Production_output",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Production_output_bmi_code_repack",
                table: "Production_output",
                column: "bmi_code_repack");

            migrationBuilder.AddForeignKey(
                name: "FK_Production_output_Master_BMI_bmi_code_repack",
                table: "Production_output",
                column: "bmi_code_repack",
                principalTable: "Master_BMI",
                principalColumn: "bmi_code",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
