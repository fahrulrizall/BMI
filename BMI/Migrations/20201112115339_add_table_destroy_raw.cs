using Microsoft.EntityFrameworkCore.Migrations;

namespace BMI.Migrations
{
    public partial class add_table_destroy_raw : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Destroy");

            migrationBuilder.CreateTable(
                name: "DestroyFG",
                columns: table => new
                {
                    id_destroyFG = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    bmi_code = table.Column<string>(nullable: true),
                    raw_source = table.Column<string>(nullable: true),
                    qty = table.Column<int>(nullable: false),
                    id_pt = table.Column<string>(nullable: true),
                    reason = table.Column<string>(nullable: true),
                    status = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DestroyFG", x => x.id_destroyFG);
                    table.ForeignKey(
                        name: "FK_DestroyFG_Master_BMI_bmi_code",
                        column: x => x.bmi_code,
                        principalTable: "Master_BMI",
                        principalColumn: "bmi_code",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DestroyFG_Pt_id_pt",
                        column: x => x.id_pt,
                        principalTable: "Pt",
                        principalColumn: "id_pt",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "DestroyRaw",
                columns: table => new
                {
                    id_destroyRaw = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    raw_source = table.Column<string>(nullable: true),
                    sap_code = table.Column<string>(nullable: true),
                    qty = table.Column<double>(nullable: false),
                    reason = table.Column<string>(nullable: true),
                    status = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DestroyRaw", x => x.id_destroyRaw);
                    table.ForeignKey(
                        name: "FK_DestroyRaw_Master_data_sap_code",
                        column: x => x.sap_code,
                        principalTable: "Master_data",
                        principalColumn: "sap_code",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DestroyFG_bmi_code",
                table: "DestroyFG",
                column: "bmi_code");

            migrationBuilder.CreateIndex(
                name: "IX_DestroyFG_id_pt",
                table: "DestroyFG",
                column: "id_pt");

            migrationBuilder.CreateIndex(
                name: "IX_DestroyRaw_sap_code",
                table: "DestroyRaw",
                column: "sap_code");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DestroyFG");

            migrationBuilder.DropTable(
                name: "DestroyRaw");

            migrationBuilder.CreateTable(
                name: "Destroy",
                columns: table => new
                {
                    id_destroy = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    bmi_code = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    id_pt = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    qty = table.Column<int>(type: "int", nullable: false),
                    raw_source = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    reason = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    status = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Destroy", x => x.id_destroy);
                    table.ForeignKey(
                        name: "FK_Destroy_Master_BMI_bmi_code",
                        column: x => x.bmi_code,
                        principalTable: "Master_BMI",
                        principalColumn: "bmi_code",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Destroy_Pt_id_pt",
                        column: x => x.id_pt,
                        principalTable: "Pt",
                        principalColumn: "id_pt",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Destroy_bmi_code",
                table: "Destroy",
                column: "bmi_code");

            migrationBuilder.CreateIndex(
                name: "IX_Destroy_id_pt",
                table: "Destroy",
                column: "id_pt");
        }
    }
}
