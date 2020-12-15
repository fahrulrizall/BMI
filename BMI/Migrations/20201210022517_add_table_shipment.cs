using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BMI.Migrations
{
    public partial class add_table_shipment : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Shipment",
                columns: table => new
                {
                    id_shipment = table.Column<string>(nullable: false),
                    po = table.Column<string>(nullable: true),
                    bmi_code = table.Column<string>(nullable: true),
                    batch = table.Column<string>(nullable: true),
                    index = table.Column<float>(nullable: false),
                    saved = table.Column<string>(nullable: true),
                    created_at = table.Column<DateTime>(nullable: false),
                    updated_at = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Shipment", x => x.id_shipment);
                    table.ForeignKey(
                        name: "FK_Shipment_Master_BMI_bmi_code",
                        column: x => x.bmi_code,
                        principalTable: "Master_BMI",
                        principalColumn: "bmi_code",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Shipment_PO_po",
                        column: x => x.po,
                        principalTable: "PO",
                        principalColumn: "po",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Shipment_detail",
                columns: table => new
                {
                    id_shipment_detail = table.Column<string>(nullable: false),
                    id_shipment = table.Column<string>(nullable: true),
                    pdc = table.Column<DateTime>(nullable: false),
                    raw_source = table.Column<string>(nullable: true),
                    landing_site = table.Column<string>(nullable: true),
                    qty = table.Column<int>(nullable: false),
                    CS_location = table.Column<string>(nullable: true),
                    created_by = table.Column<string>(nullable: true),
                    updated_by = table.Column<string>(nullable: true),
                    created_at = table.Column<DateTime>(nullable: true),
                    updated_at = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Shipment_detail", x => x.id_shipment_detail);
                    table.ForeignKey(
                        name: "FK_Shipment_detail_Shipment_id_shipment",
                        column: x => x.id_shipment,
                        principalTable: "Shipment",
                        principalColumn: "id_shipment",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Shipment_bmi_code",
                table: "Shipment",
                column: "bmi_code");

            migrationBuilder.CreateIndex(
                name: "IX_Shipment_po",
                table: "Shipment",
                column: "po");

            migrationBuilder.CreateIndex(
                name: "IX_Shipment_detail_id_shipment",
                table: "Shipment_detail",
                column: "id_shipment");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Shipment_detail");

            migrationBuilder.DropTable(
                name: "Shipment");
        }
    }
}
