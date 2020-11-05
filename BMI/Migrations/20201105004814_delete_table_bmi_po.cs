using Microsoft.EntityFrameworkCore.Migrations;

namespace BMI.Migrations
{
    public partial class delete_table_bmi_po : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Production_input_Bmi_po_po",
                table: "Production_input");

            migrationBuilder.DropForeignKey(
                name: "FK_Production_output_Bmi_po_po",
                table: "Production_output");

            migrationBuilder.DropTable(
                name: "Bmi_po");

            migrationBuilder.DropIndex(
                name: "IX_Production_output_po",
                table: "Production_output");

            migrationBuilder.DropIndex(
                name: "IX_Production_input_po",
                table: "Production_input");

            migrationBuilder.AlterColumn<string>(
                name: "po",
                table: "Production_output",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "po",
                table: "Production_input",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "po",
                table: "Production_output",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "po",
                table: "Production_input",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "Bmi_po",
                columns: table => new
                {
                    po = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    pt = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bmi_po", x => x.po);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Production_output_po",
                table: "Production_output",
                column: "po");

            migrationBuilder.CreateIndex(
                name: "IX_Production_input_po",
                table: "Production_input",
                column: "po");

            migrationBuilder.AddForeignKey(
                name: "FK_Production_input_Bmi_po_po",
                table: "Production_input",
                column: "po",
                principalTable: "Bmi_po",
                principalColumn: "po",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Production_output_Bmi_po_po",
                table: "Production_output",
                column: "po",
                principalTable: "Bmi_po",
                principalColumn: "po",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
