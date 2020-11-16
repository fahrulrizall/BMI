using Microsoft.EntityFrameworkCore.Migrations;

namespace BMI.Migrations
{
    public partial class change_column_name_on_table_rm : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Production_output_Master_BMI_MasterBMIModel1bmi_code",
                table: "Production_output");

            migrationBuilder.DropIndex(
                name: "IX_Production_output_MasterBMIModel1bmi_code",
                table: "Production_output");

            migrationBuilder.DropColumn(
                name: "reff",
                table: "Rm");

            migrationBuilder.DropColumn(
                name: "MasterBMIModel1bmi_code",
                table: "Production_output");

            migrationBuilder.AddColumn<string>(
                name: "raw_source",
                table: "Rm",
                nullable: true);

            migrationBuilder.AlterColumn<float>(
                name: "lbs",
                table: "Master_BMI",
                nullable: false,
                oldClrType: typeof(float),
                oldType: "real",
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "raw_source",
                table: "Rm");

            migrationBuilder.AddColumn<string>(
                name: "reff",
                table: "Rm",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MasterBMIModel1bmi_code",
                table: "Production_output",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AlterColumn<float>(
                name: "lbs",
                table: "Master_BMI",
                type: "real",
                nullable: true,
                oldClrType: typeof(float));

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
    }
}
