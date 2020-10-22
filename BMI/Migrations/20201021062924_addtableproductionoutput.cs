using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BMI.Migrations
{
    public partial class addtableproductionoutput : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Production_output",
                columns: table => new
                {
                    id_productionoutput = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    po = table.Column<string>(nullable: true),
                    date = table.Column<DateTime>(nullable: false),
                    pt = table.Column<int>(nullable: false),
                    bmi_code = table.Column<string>(nullable: true),
                    qty = table.Column<float>(nullable: false),
                    batch = table.Column<string>(nullable: true),
                    bmi_code_repack = table.Column<string>(nullable: true),
                    batch_repack = table.Column<string>(nullable: true),
                    saved = table.Column<string>(nullable: true),
                    created_at = table.Column<DateTime>(nullable: true),
                    updated_at = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Production_output", x => x.id_productionoutput);
                    table.ForeignKey(
                        name: "FK_Production_output_Master_BMI_bmi_code",
                        column: x => x.bmi_code,
                        principalTable: "Master_BMI",
                        principalColumn: "bmi_code",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Production_output_Master_BMI_bmi_code_repack",
                        column: x => x.bmi_code_repack,
                        principalTable: "Master_BMI",
                        principalColumn: "bmi_code",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Production_output_bmi_code",
                table: "Production_output",
                column: "bmi_code");

            migrationBuilder.CreateIndex(
                name: "IX_Production_output_bmi_code_repack",
                table: "Production_output",
                column: "bmi_code_repack");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Production_output");
        }
    }
}
