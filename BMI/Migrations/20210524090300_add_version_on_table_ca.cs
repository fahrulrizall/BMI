using Microsoft.EntityFrameworkCore.Migrations;

namespace BMI.Migrations
{
    public partial class add_version_on_table_ca : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "SAP_Code",
                table: "CostAnalyst",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Version",
                table: "CostAnalyst",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_CostAnalyst_SAP_Code",
                table: "CostAnalyst",
                column: "SAP_Code");

            migrationBuilder.AddForeignKey(
                name: "FK_CostAnalyst_Master_data_SAP_Code",
                table: "CostAnalyst",
                column: "SAP_Code",
                principalTable: "Master_data",
                principalColumn: "sap_code",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CostAnalyst_Master_data_SAP_Code",
                table: "CostAnalyst");

            migrationBuilder.DropIndex(
                name: "IX_CostAnalyst_SAP_Code",
                table: "CostAnalyst");

            migrationBuilder.DropColumn(
                name: "Version",
                table: "CostAnalyst");

            migrationBuilder.AlterColumn<string>(
                name: "SAP_Code",
                table: "CostAnalyst",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);
        }
    }
}
