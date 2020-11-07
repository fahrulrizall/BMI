using Microsoft.EntityFrameworkCore.Migrations;

namespace BMI.Migrations
{
    public partial class change_int_to_string_on_key_table_pt : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Production_input_pt_id_pt",
                table: "Production_input");

            migrationBuilder.DropForeignKey(
                name: "FK_Production_output_pt_id_pt",
                table: "Production_output");

            migrationBuilder.DropTable(
                name: "pt");

            migrationBuilder.DropIndex(
                name: "IX_Production_output_id_pt",
                table: "Production_output");

            migrationBuilder.DropIndex(
                name: "IX_Production_input_id_pt",
                table: "Production_input");

            migrationBuilder.AlterColumn<string>(
                name: "id_pt",
                table: "Production_output",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "id_pt",
                table: "Production_input",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "id_pt",
                table: "Production_output",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "id_pt",
                table: "Production_input",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "pt",
                columns: table => new
                {
                    id_pt = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    batch = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    plant = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    po = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    pt = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_pt", x => x.id_pt);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Production_output_id_pt",
                table: "Production_output",
                column: "id_pt");

            migrationBuilder.CreateIndex(
                name: "IX_Production_input_id_pt",
                table: "Production_input",
                column: "id_pt");

            migrationBuilder.AddForeignKey(
                name: "FK_Production_input_pt_id_pt",
                table: "Production_input",
                column: "id_pt",
                principalTable: "pt",
                principalColumn: "id_pt",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Production_output_pt_id_pt",
                table: "Production_output",
                column: "id_pt",
                principalTable: "pt",
                principalColumn: "id_pt",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
