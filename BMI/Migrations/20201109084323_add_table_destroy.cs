using Microsoft.EntityFrameworkCore.Migrations;

namespace BMI.Migrations
{
    public partial class add_table_destroy : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "destroy",
                columns: table => new
                {
                    id_destroy = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    bmi_code = table.Column<string>(nullable: true),
                    qty = table.Column<float>(nullable: false),
                    pt = table.Column<string>(nullable: true),
                    reason = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_destroy", x => x.id_destroy);
                    table.ForeignKey(
                        name: "FK_destroy_Master_BMI_bmi_code",
                        column: x => x.bmi_code,
                        principalTable: "Master_BMI",
                        principalColumn: "bmi_code",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_destroy_pt_pt",
                        column: x => x.pt,
                        principalTable: "pt",
                        principalColumn: "id_pt",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_destroy_bmi_code",
                table: "destroy",
                column: "bmi_code");

            migrationBuilder.CreateIndex(
                name: "IX_destroy_pt",
                table: "destroy",
                column: "pt");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "destroy");
        }
    }
}
