using Microsoft.EntityFrameworkCore.Migrations;

namespace BMI.Migrations
{
    public partial class add_table_pt : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "pt",
                columns: table => new
                {
                    id_pt = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
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
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Production_output_pt_id_pt",
                table: "Production_output",
                column: "id_pt",
                principalTable: "pt",
                principalColumn: "id_pt",
                onDelete: ReferentialAction.Cascade);
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
        }
    }
}
