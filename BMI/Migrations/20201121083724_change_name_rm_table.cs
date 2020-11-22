using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BMI.Migrations
{
    public partial class change_name_rm_table : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Rm");

            migrationBuilder.CreateTable(
                name: "Rm_detail",
                columns: table => new
                {
                    id_raw = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    etd = table.Column<DateTime>(nullable: false),
                    eta = table.Column<DateTime>(nullable: false),
                    container = table.Column<string>(nullable: true),
                    raw_source = table.Column<string>(nullable: true),
                    status = table.Column<string>(nullable: true),
                    landing_site = table.Column<string>(nullable: true),
                    sap_code = table.Column<string>(nullable: true),
                    cases = table.Column<int>(nullable: true),
                    uom = table.Column<string>(nullable: true),
                    usd_price = table.Column<float>(nullable: true),
                    ex_rate = table.Column<float>(nullable: true),
                    CS_location = table.Column<string>(nullable: true),
                    qty_pl = table.Column<float>(nullable: true),
                    qty_received = table.Column<float>(nullable: true),
                    saved = table.Column<string>(nullable: true),
                    created_at = table.Column<DateTime>(nullable: true),
                    updated_at = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rm_detail", x => x.id_raw);
                    table.ForeignKey(
                        name: "FK_Rm_detail_Master_data_sap_code",
                        column: x => x.sap_code,
                        principalTable: "Master_data",
                        principalColumn: "sap_code",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Rm_detail_sap_code",
                table: "Rm_detail",
                column: "sap_code");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Rm_detail");

            migrationBuilder.CreateTable(
                name: "Rm",
                columns: table => new
                {
                    id_raw = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CS_location = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    cases = table.Column<int>(type: "int", nullable: true),
                    container = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: true),
                    eta = table.Column<DateTime>(type: "datetime2", nullable: false),
                    etd = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ex_rate = table.Column<float>(type: "real", nullable: true),
                    landing_site = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    qty_pl = table.Column<float>(type: "real", nullable: true),
                    qty_received = table.Column<float>(type: "real", nullable: true),
                    raw_source = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    sap_code = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    saved = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    status = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    uom = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    updated_at = table.Column<DateTime>(type: "datetime2", nullable: true),
                    usd_price = table.Column<float>(type: "real", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rm", x => x.id_raw);
                    table.ForeignKey(
                        name: "FK_Rm_Master_data_sap_code",
                        column: x => x.sap_code,
                        principalTable: "Master_data",
                        principalColumn: "sap_code",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Rm_sap_code",
                table: "Rm",
                column: "sap_code");
        }
    }
}
