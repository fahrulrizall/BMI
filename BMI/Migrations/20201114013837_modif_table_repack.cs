using Microsoft.EntityFrameworkCore.Migrations;

namespace BMI.Migrations
{
    public partial class modif_table_repack : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Repack_Master_BMI_MasterBMIModelbmi_code",
                table: "Repack");

            migrationBuilder.DropForeignKey(
                name: "FK_Repack_Pt_PTModelid_pt",
                table: "Repack");

            migrationBuilder.DropIndex(
                name: "IX_Repack_MasterBMIModelbmi_code",
                table: "Repack");

            migrationBuilder.DropIndex(
                name: "IX_Repack_PTModelid_pt",
                table: "Repack");

            migrationBuilder.DropColumn(
                name: "MasterBMIModelbmi_code",
                table: "Repack");

            migrationBuilder.DropColumn(
                name: "PTModelid_pt",
                table: "Repack");

            migrationBuilder.AlterColumn<string>(
                name: "to_pt",
                table: "Repack",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "to_bmi_code",
                table: "Repack",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "from_pt",
                table: "Repack",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "from_bmi_code",
                table: "Repack",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Repack_from_bmi_code",
                table: "Repack",
                column: "from_bmi_code");

            migrationBuilder.CreateIndex(
                name: "IX_Repack_from_pt",
                table: "Repack",
                column: "from_pt");

            migrationBuilder.CreateIndex(
                name: "IX_Repack_to_bmi_code",
                table: "Repack",
                column: "to_bmi_code");

            migrationBuilder.CreateIndex(
                name: "IX_Repack_to_pt",
                table: "Repack",
                column: "to_pt");

            migrationBuilder.AddForeignKey(
                name: "FK_Repack_Master_BMI_from_bmi_code",
                table: "Repack",
                column: "from_bmi_code",
                principalTable: "Master_BMI",
                principalColumn: "bmi_code",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Repack_Pt_from_pt",
                table: "Repack",
                column: "from_pt",
                principalTable: "Pt",
                principalColumn: "id_pt",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Repack_Master_BMI_to_bmi_code",
                table: "Repack",
                column: "to_bmi_code",
                principalTable: "Master_BMI",
                principalColumn: "bmi_code",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Repack_Pt_to_pt",
                table: "Repack",
                column: "to_pt",
                principalTable: "Pt",
                principalColumn: "id_pt",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Repack_Master_BMI_from_bmi_code",
                table: "Repack");

            migrationBuilder.DropForeignKey(
                name: "FK_Repack_Pt_from_pt",
                table: "Repack");

            migrationBuilder.DropForeignKey(
                name: "FK_Repack_Master_BMI_to_bmi_code",
                table: "Repack");

            migrationBuilder.DropForeignKey(
                name: "FK_Repack_Pt_to_pt",
                table: "Repack");

            migrationBuilder.DropIndex(
                name: "IX_Repack_from_bmi_code",
                table: "Repack");

            migrationBuilder.DropIndex(
                name: "IX_Repack_from_pt",
                table: "Repack");

            migrationBuilder.DropIndex(
                name: "IX_Repack_to_bmi_code",
                table: "Repack");

            migrationBuilder.DropIndex(
                name: "IX_Repack_to_pt",
                table: "Repack");

            migrationBuilder.AlterColumn<string>(
                name: "to_pt",
                table: "Repack",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "to_bmi_code",
                table: "Repack",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "from_pt",
                table: "Repack",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "from_bmi_code",
                table: "Repack",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MasterBMIModelbmi_code",
                table: "Repack",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PTModelid_pt",
                table: "Repack",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Repack_MasterBMIModelbmi_code",
                table: "Repack",
                column: "MasterBMIModelbmi_code");

            migrationBuilder.CreateIndex(
                name: "IX_Repack_PTModelid_pt",
                table: "Repack",
                column: "PTModelid_pt");

            migrationBuilder.AddForeignKey(
                name: "FK_Repack_Master_BMI_MasterBMIModelbmi_code",
                table: "Repack",
                column: "MasterBMIModelbmi_code",
                principalTable: "Master_BMI",
                principalColumn: "bmi_code",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Repack_Pt_PTModelid_pt",
                table: "Repack",
                column: "PTModelid_pt",
                principalTable: "Pt",
                principalColumn: "id_pt",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
