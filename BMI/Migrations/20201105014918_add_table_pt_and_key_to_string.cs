using Microsoft.EntityFrameworkCore.Migrations;

namespace BMI.Migrations
{
    public partial class add_table_pt_and_key_to_string : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "id_pt",
                table: "Production_output",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "id_pt",
                table: "Production_input",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "pt",
                columns: table => new
                {
                    id_pt = table.Column<string>(nullable: false),
                    pt = table.Column<int>(nullable: false),
                    plant = table.Column<string>(nullable: true),
                    batch = table.Column<string>(nullable: true),
                    po = table.Column<string>(nullable: true)
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
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Production_output_pt_id_pt",
                table: "Production_output",
                column: "id_pt",
                principalTable: "pt",
                principalColumn: "id_pt",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
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
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "id_pt",
                table: "Production_input",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);
        }
    }
}
