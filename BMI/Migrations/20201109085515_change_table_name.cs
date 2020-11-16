using Microsoft.EntityFrameworkCore.Migrations;

namespace BMI.Migrations
{
    public partial class change_table_name : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_destroy_Master_BMI_bmi_code",
                table: "destroy");

            migrationBuilder.DropForeignKey(
                name: "FK_destroy_pt_pt",
                table: "destroy");

            migrationBuilder.DropForeignKey(
                name: "FK_Production_input_pt_id_pt",
                table: "Production_input");

            migrationBuilder.DropForeignKey(
                name: "FK_Production_output_pt_id_pt",
                table: "Production_output");

            migrationBuilder.DropPrimaryKey(
                name: "PK_pt",
                table: "pt");

            migrationBuilder.DropPrimaryKey(
                name: "PK_destroy",
                table: "destroy");

            migrationBuilder.RenameTable(
                name: "pt",
                newName: "Pt");

            migrationBuilder.RenameTable(
                name: "destroy",
                newName: "Destroy");

            migrationBuilder.RenameIndex(
                name: "IX_destroy_pt",
                table: "Destroy",
                newName: "IX_Destroy_pt");

            migrationBuilder.RenameIndex(
                name: "IX_destroy_bmi_code",
                table: "Destroy",
                newName: "IX_Destroy_bmi_code");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Pt",
                table: "Pt",
                column: "id_pt");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Destroy",
                table: "Destroy",
                column: "id_destroy");

            migrationBuilder.AddForeignKey(
                name: "FK_Destroy_Master_BMI_bmi_code",
                table: "Destroy",
                column: "bmi_code",
                principalTable: "Master_BMI",
                principalColumn: "bmi_code",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Destroy_Pt_pt",
                table: "Destroy",
                column: "pt",
                principalTable: "Pt",
                principalColumn: "id_pt",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Production_input_Pt_id_pt",
                table: "Production_input",
                column: "id_pt",
                principalTable: "Pt",
                principalColumn: "id_pt",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Production_output_Pt_id_pt",
                table: "Production_output",
                column: "id_pt",
                principalTable: "Pt",
                principalColumn: "id_pt",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Destroy_Master_BMI_bmi_code",
                table: "Destroy");

            migrationBuilder.DropForeignKey(
                name: "FK_Destroy_Pt_pt",
                table: "Destroy");

            migrationBuilder.DropForeignKey(
                name: "FK_Production_input_Pt_id_pt",
                table: "Production_input");

            migrationBuilder.DropForeignKey(
                name: "FK_Production_output_Pt_id_pt",
                table: "Production_output");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Pt",
                table: "Pt");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Destroy",
                table: "Destroy");

            migrationBuilder.RenameTable(
                name: "Pt",
                newName: "pt");

            migrationBuilder.RenameTable(
                name: "Destroy",
                newName: "destroy");

            migrationBuilder.RenameIndex(
                name: "IX_Destroy_pt",
                table: "destroy",
                newName: "IX_destroy_pt");

            migrationBuilder.RenameIndex(
                name: "IX_Destroy_bmi_code",
                table: "destroy",
                newName: "IX_destroy_bmi_code");

            migrationBuilder.AddPrimaryKey(
                name: "PK_pt",
                table: "pt",
                column: "id_pt");

            migrationBuilder.AddPrimaryKey(
                name: "PK_destroy",
                table: "destroy",
                column: "id_destroy");

            migrationBuilder.AddForeignKey(
                name: "FK_destroy_Master_BMI_bmi_code",
                table: "destroy",
                column: "bmi_code",
                principalTable: "Master_BMI",
                principalColumn: "bmi_code",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_destroy_pt_pt",
                table: "destroy",
                column: "pt",
                principalTable: "pt",
                principalColumn: "id_pt",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Production_input_pt_id_pt",
                table: "Production_input",
                column: "id_pt",
                principalTable: "pt",
                principalColumn: "id_pt",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Production_output_pt_id_pt",
                table: "Production_output",
                column: "id_pt",
                principalTable: "pt",
                principalColumn: "id_pt",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
