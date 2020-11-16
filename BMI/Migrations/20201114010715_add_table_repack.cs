using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BMI.Migrations
{
    public partial class add_table_repack : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Repack",
                columns: table => new
                {
                    id_repack = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    po = table.Column<string>(nullable: true),
                    date = table.Column<DateTime>(nullable: false),
                    raw_source = table.Column<string>(nullable: true),
                    qty = table.Column<float>(nullable: false),
                    production_date = table.Column<DateTime>(nullable: false),
                    from_pt = table.Column<string>(nullable: true),
                    from_bmi_code = table.Column<string>(nullable: true),
                    to_pt = table.Column<string>(nullable: true),
                    to_bmi_code = table.Column<string>(nullable: true),
                    PTModelid_pt = table.Column<string>(nullable: true),
                    MasterBMIModelbmi_code = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Repack", x => x.id_repack);
                    table.ForeignKey(
                        name: "FK_Repack_Master_BMI_MasterBMIModelbmi_code",
                        column: x => x.MasterBMIModelbmi_code,
                        principalTable: "Master_BMI",
                        principalColumn: "bmi_code",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Repack_Pt_PTModelid_pt",
                        column: x => x.PTModelid_pt,
                        principalTable: "Pt",
                        principalColumn: "id_pt",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Repack_MasterBMIModelbmi_code",
                table: "Repack",
                column: "MasterBMIModelbmi_code");

            migrationBuilder.CreateIndex(
                name: "IX_Repack_PTModelid_pt",
                table: "Repack",
                column: "PTModelid_pt");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Repack");
        }
    }
}
