using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BMI.Migrations
{
    public partial class addtableproductioninput : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Production_input",
                columns: table => new
                {
                    id_productioninput = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    po = table.Column<int>(nullable: false),
                    date = table.Column<DateTime>(nullable: false),
                    pt = table.Column<int>(nullable: false),
                    reff = table.Column<string>(nullable: true),
                    bmi_code = table.Column<string>(nullable: true),
                    qty = table.Column<float>(nullable: false),
                    landing_site = table.Column<string>(nullable: true),
                    saved = table.Column<string>(nullable: true),
                    created_at = table.Column<DateTime>(nullable: true),
                    updated_at = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Production_input", x => x.id_productioninput);
                    table.ForeignKey(
                        name: "FK_Production_input_Master_BMI_bmi_code",
                        column: x => x.bmi_code,
                        principalTable: "Master_BMI",
                        principalColumn: "bmi_code",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Production_input_bmi_code",
                table: "Production_input",
                column: "bmi_code");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Production_input");
        }
    }
}
