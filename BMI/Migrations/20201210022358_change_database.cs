using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BMI.Migrations
{
    public partial class change_database : Migration
    {
        //saat jalankan migration ini harap komentarin pada table shipment dan shipment detail
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Shipment_detail");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Shipment_detail",
                columns: table => new
                {
                    id_shipment_detail = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    batch = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    bmi_code = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: false),
                    index = table.Column<float>(type: "real", nullable: false),
                    landing_site = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    pdc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    po = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    qty = table.Column<int>(type: "int", nullable: false),
                    raw_source = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    saved = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    updated_at = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Shipment_detail", x => x.id_shipment_detail);
                    table.ForeignKey(
                        name: "FK_Shipment_detail_Master_BMI_bmi_code",
                        column: x => x.bmi_code,
                        principalTable: "Master_BMI",
                        principalColumn: "bmi_code",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Shipment_detail_PO_po",
                        column: x => x.po,
                        principalTable: "PO",
                        principalColumn: "po",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Shipment_detail_bmi_code",
                table: "Shipment_detail",
                column: "bmi_code");

            migrationBuilder.CreateIndex(
                name: "IX_Shipment_detail_po",
                table: "Shipment_detail",
                column: "po");
        }
    }
}
