using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BMI.Migrations
{
    public partial class add_complete_table : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CostAnalyst",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PO = table.Column<string>(nullable: true),
                    SAP_Code = table.Column<string>(nullable: true),
                    Target_Lbs = table.Column<float>(nullable: false),
                    Version = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CostAnalyst", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CostAnalyst_PO_PO",
                        column: x => x.PO,
                        principalTable: "PO",
                        principalColumn: "po",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CostAnalyst_Master_data_SAP_Code",
                        column: x => x.SAP_Code,
                        principalTable: "Master_data",
                        principalColumn: "sap_code",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Rm_Cost",
                columns: table => new
                {
                    Id_Material = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Material = table.Column<string>(nullable: true),
                    PO = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rm_Cost", x => x.Id_Material);
                    table.ForeignKey(
                        name: "FK_Rm_Cost_PO_PO",
                        column: x => x.PO,
                        principalTable: "PO",
                        principalColumn: "po",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SAP_PO",
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
                    table.PrimaryKey("PK_SAP_PO", x => x.refference);
                    table.ForeignKey(
                        name: "FK_SAP_PO_Vendor_vendor",
                        column: x => x.vendor,
                        principalTable: "Vendor",
                        principalColumn: "code",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Date_vessel",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    date = table.Column<DateTime>(nullable: false),
                    refference = table.Column<string>(nullable: true),
                    vessel = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Date_vessel", x => x.id);
                    table.ForeignKey(
                        name: "FK_Date_vessel_SAP_PO_refference",
                        column: x => x.refference,
                        principalTable: "SAP_PO",
                        principalColumn: "refference",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SAP_PO_Detail",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
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
                    table.PrimaryKey("PK_SAP_PO_Detail", x => x.id);
                    table.ForeignKey(
                        name: "FK_SAP_PO_Detail_SAP_PO_refference",
                        column: x => x.refference,
                        principalTable: "SAP_PO",
                        principalColumn: "refference",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SAP_PO_Detail_Master_data_sap_code",
                        column: x => x.sap_code,
                        principalTable: "Master_data",
                        principalColumn: "sap_code",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "QtyLine1Input",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    id_dateVessel = table.Column<int>(nullable: false),
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
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "QtyLine1Output",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    sap_code = table.Column<string>(nullable: true),
                    datevesselmodelid = table.Column<int>(nullable: true),
                    id_dateVessel = table.Column<int>(nullable: false),
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
                name: "IX_CostAnalyst_PO",
                table: "CostAnalyst",
                column: "PO");

            migrationBuilder.CreateIndex(
                name: "IX_CostAnalyst_SAP_Code",
                table: "CostAnalyst",
                column: "SAP_Code");

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
                name: "IX_Rm_Cost_PO",
                table: "Rm_Cost",
                column: "PO");

            migrationBuilder.CreateIndex(
                name: "IX_SAP_PO_vendor",
                table: "SAP_PO",
                column: "vendor");

            migrationBuilder.CreateIndex(
                name: "IX_SAP_PO_Detail_refference",
                table: "SAP_PO_Detail",
                column: "refference");

            migrationBuilder.CreateIndex(
                name: "IX_SAP_PO_Detail_sap_code",
                table: "SAP_PO_Detail",
                column: "sap_code");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CostAnalyst");

            migrationBuilder.DropTable(
                name: "QtyLine1Input");

            migrationBuilder.DropTable(
                name: "QtyLine1Output");

            migrationBuilder.DropTable(
                name: "Rm_Cost");

            migrationBuilder.DropTable(
                name: "SAP_PO_Detail");

            migrationBuilder.DropTable(
                name: "Date_vessel");

            migrationBuilder.DropTable(
                name: "SAP_PO");
        }
    }
}
