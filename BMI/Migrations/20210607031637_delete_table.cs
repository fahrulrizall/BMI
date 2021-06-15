using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BMI.Migrations
{
    public partial class delete_table : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
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
                name: "Rm_plant_detail");

            migrationBuilder.DropTable(
                name: "Date_vessel");

            migrationBuilder.DropTable(
                name: "Rm_plant");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CostAnalyst",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PO = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    SAP_Code = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    Target_Lbs = table.Column<float>(type: "real", nullable: false),
                    Version = table.Column<string>(type: "nvarchar(max)", nullable: true)
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
                    Id_Material = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Material = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PO = table.Column<string>(type: "nvarchar(450)", nullable: true)
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
                name: "Rm_plant",
                columns: table => new
                {
                    refference = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    bl_no = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    container = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    delivery_date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    destination = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    eta = table.Column<DateTime>(type: "datetime2", nullable: false),
                    etd = table.Column<DateTime>(type: "datetime2", nullable: false),
                    invoice = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    loading_port = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    pgi = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    pgr = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    pgr_date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    plant = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    return_no = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    sap_po = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    shipping_line = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    status = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    vendor = table.Column<string>(type: "nvarchar(450)", nullable: true)
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
                    id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    refference = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    vessel = table.Column<string>(type: "nvarchar(max)", nullable: true)
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
                    id_rmmowidetail = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    qty_pl = table.Column<float>(type: "real", nullable: false),
                    qty_received = table.Column<float>(type: "real", nullable: false),
                    refference = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    sap_code = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    style = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    unit_price = table.Column<float>(type: "real", nullable: false),
                    vessel = table.Column<string>(type: "nvarchar(max)", nullable: true)
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
                    id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    id_dateVessel = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    qty = table.Column<float>(type: "real", nullable: false),
                    refference = table.Column<string>(type: "nvarchar(max)", nullable: true)
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
                    id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    batch = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    datevesselmodelid = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    id_dateVessel = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    qty = table.Column<float>(type: "real", nullable: false),
                    refference = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    sap_code = table.Column<string>(type: "nvarchar(450)", nullable: true)
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
    }
}
