using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BMI.Migrations
{
    public partial class mowi_part1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Vendor",
                columns: table => new
                {
                    code = table.Column<string>(nullable: false),
                    description = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Vendor", x => x.code);
                });

            migrationBuilder.CreateTable(
                name: "Rm_plant",
                columns: table => new
                {
                    refference = table.Column<string>(nullable: false),
                    vendor = table.Column<string>(nullable: true),
                    sap_po = table.Column<string>(nullable: true),
                    pgi = table.Column<string>(nullable: true),
                    pgr = table.Column<string>(nullable: true),
                    return_no = table.Column<string>(nullable: true),
                    plant = table.Column<string>(nullable: true),
                    delivery_date = table.Column<DateTime>(nullable: false),
                    etd = table.Column<DateTime>(nullable: false),
                    eta = table.Column<DateTime>(nullable: false),
                    invoice = table.Column<string>(nullable: true),
                    container = table.Column<string>(nullable: true),
                    bl_no = table.Column<string>(nullable: true),
                    shipping_line = table.Column<string>(nullable: true),
                    loading_port = table.Column<string>(nullable: true),
                    destination = table.Column<string>(nullable: true),
                    pgr_date = table.Column<DateTime>(nullable: false),
                    status = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rm_plant", x => x.refference);
                    table.ForeignKey(
                        name: "FK_Rm_plant_Vendor_vendor",
                        column: x => x.vendor,
                        principalTable: "Vendor",
                        principalColumn: "code",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Date_vessel",
                columns: table => new
                {
                    id = table.Column<string>(nullable: false),
                    date = table.Column<DateTime>(nullable: false),
                    refference = table.Column<string>(nullable: true),
                    vessel = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Date_vessel", x => x.id);
                    table.ForeignKey(
                        name: "FK_Date_vessel_Rm_plant_refference",
                        column: x => x.refference,
                        principalTable: "Rm_plant",
                        principalColumn: "refference",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Rm_plant_detail",
                columns: table => new
                {
                    id_rmmowidetail = table.Column<string>(nullable: false),
                    refference = table.Column<string>(nullable: true),
                    sap_code = table.Column<string>(nullable: true),
                    style = table.Column<string>(nullable: true),
                    unit_price = table.Column<float>(nullable: false),
                    qty_pl = table.Column<float>(nullable: false),
                    qty_received = table.Column<float>(nullable: false),
                    vessel = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rm_plant_detail", x => x.id_rmmowidetail);
                    table.ForeignKey(
                        name: "FK_Rm_plant_detail_Rm_plant_refference",
                        column: x => x.refference,
                        principalTable: "Rm_plant",
                        principalColumn: "refference",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Rm_plant_detail_Master_data_sap_code",
                        column: x => x.sap_code,
                        principalTable: "Master_data",
                        principalColumn: "sap_code",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "QtyLine1Input",
                columns: table => new
                {
                    id = table.Column<string>(nullable: false),
                    id_dateVessel = table.Column<string>(nullable: true),
                    refference = table.Column<string>(nullable: true),
                    qty = table.Column<float>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QtyLine1Input", x => x.id);
                    table.ForeignKey(
                        name: "FK_QtyLine1Input_Date_vessel_id_dateVessel",
                        column: x => x.id_dateVessel,
                        principalTable: "Date_vessel",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "QtyLine1Output",
                columns: table => new
                {
                    id = table.Column<string>(nullable: false),
                    sap_code = table.Column<string>(nullable: true),
                    datevesselmodelid = table.Column<string>(nullable: true),
                    id_dateVessel = table.Column<string>(nullable: true),
                    refference = table.Column<string>(nullable: true),
                    qty = table.Column<float>(nullable: false),
                    batch = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QtyLine1Output", x => x.id);
                    table.ForeignKey(
                        name: "FK_QtyLine1Output_Date_vessel_datevesselmodelid",
                        column: x => x.datevesselmodelid,
                        principalTable: "Date_vessel",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_QtyLine1Output_Master_data_sap_code",
                        column: x => x.sap_code,
                        principalTable: "Master_data",
                        principalColumn: "sap_code",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Date_vessel_refference",
                table: "Date_vessel",
                column: "refference");

            migrationBuilder.CreateIndex(
                name: "IX_QtyLine1Input_id_dateVessel",
                table: "QtyLine1Input",
                column: "id_dateVessel");

            migrationBuilder.CreateIndex(
                name: "IX_QtyLine1Output_datevesselmodelid",
                table: "QtyLine1Output",
                column: "datevesselmodelid");

            migrationBuilder.CreateIndex(
                name: "IX_QtyLine1Output_sap_code",
                table: "QtyLine1Output",
                column: "sap_code");

            migrationBuilder.CreateIndex(
                name: "IX_Rm_plant_vendor",
                table: "Rm_plant",
                column: "vendor");

            migrationBuilder.CreateIndex(
                name: "IX_Rm_plant_detail_refference",
                table: "Rm_plant_detail",
                column: "refference");

            migrationBuilder.CreateIndex(
                name: "IX_Rm_plant_detail_sap_code",
                table: "Rm_plant_detail",
                column: "sap_code");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "QtyLine1Input");

            migrationBuilder.DropTable(
                name: "QtyLine1Output");

            migrationBuilder.DropTable(
                name: "Rm_plant_detail");

            migrationBuilder.DropTable(
                name: "Date_vessel");

            migrationBuilder.DropTable(
                name: "Rm_plant");

            migrationBuilder.DropTable(
                name: "Vendor");
        }
    }
}
