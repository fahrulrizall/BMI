using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BMI.Migrations
{
    public partial class all_table_server : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Master_BMI",
                columns: table => new
                {
                    bmi_code = table.Column<string>(nullable: false),
                    sap_code = table.Column<string>(nullable: true),
                    description = table.Column<string>(nullable: true),
                    lbs = table.Column<float>(nullable: false),
                    index_category = table.Column<string>(nullable: true),
                    daily_category = table.Column<string>(nullable: true),
                    weekly_category = table.Column<string>(nullable: true),
                    index = table.Column<float>(nullable: true),
                    index_lb = table.Column<float>(nullable: true),
                    index_cs = table.Column<float>(nullable: true),
                    zafc_kg = table.Column<float>(nullable: true),
                    zafc_cs = table.Column<float>(nullable: true),
                    created_at = table.Column<DateTime>(nullable: true),
                    updated_at = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Master_BMI", x => x.bmi_code);
                });

            migrationBuilder.CreateTable(
                name: "Master_data",
                columns: table => new
                {
                    sap_code = table.Column<string>(nullable: false),
                    description = table.Column<string>(nullable: true),
                    lbs = table.Column<float>(nullable: true),
                    category = table.Column<string>(nullable: true),
                    created_at = table.Column<DateTime>(nullable: true),
                    updated_at = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Master_data", x => x.sap_code);
                });

            migrationBuilder.CreateTable(
                name: "Pt",
                columns: table => new
                {
                    id_pt = table.Column<string>(nullable: false),
                    pt = table.Column<int>(nullable: false),
                    plant = table.Column<string>(nullable: true),
                    batch = table.Column<string>(nullable: true),
                    po = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pt", x => x.id_pt);
                });

            migrationBuilder.CreateTable(
                name: "Shipment",
                columns: table => new
                {
                    id_shipment = table.Column<int>(nullable: false),
                    etd = table.Column<DateTime>(nullable: false),
                    eta = table.Column<DateTime>(nullable: false),
                    destination = table.Column<string>(nullable: false),
                    po = table.Column<string>(nullable: false),
                    saved = table.Column<string>(nullable: true),
                    created_at = table.Column<DateTime>(nullable: true),
                    updated_at = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Shipment", x => x.id_shipment);
                });

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    UserId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(nullable: false),
                    LastName = table.Column<string>(nullable: false),
                    EmailAddress = table.Column<string>(nullable: false),
                    ConfirmEmail = table.Column<string>(nullable: true),
                    Password = table.Column<string>(maxLength: 100, nullable: false),
                    ConfirmPassword = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.UserId);
                });

            migrationBuilder.CreateTable(
                name: "AdjustmentRaw",
                columns: table => new
                {
                    id_adjustmentRaw = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    raw_source = table.Column<string>(nullable: true),
                    sap_code = table.Column<string>(nullable: true),
                    qty = table.Column<double>(nullable: false),
                    reason = table.Column<string>(nullable: true),
                    status = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AdjustmentRaw", x => x.id_adjustmentRaw);
                    table.ForeignKey(
                        name: "FK_AdjustmentRaw_Master_data_sap_code",
                        column: x => x.sap_code,
                        principalTable: "Master_data",
                        principalColumn: "sap_code",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Fg",
                columns: table => new
                {
                    id_fg = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    sap_code = table.Column<string>(nullable: false),
                    plant = table.Column<int>(nullable: false),
                    price_lbs = table.Column<float>(nullable: false),
                    std_price = table.Column<float>(nullable: false),
                    processing_fee = table.Column<float>(nullable: false),
                    created_at = table.Column<DateTime>(nullable: false),
                    updated_at = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Fg", x => x.id_fg);
                    table.ForeignKey(
                        name: "FK_Fg_Master_data_sap_code",
                        column: x => x.sap_code,
                        principalTable: "Master_data",
                        principalColumn: "sap_code",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Rm",
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
                    qty_pl = table.Column<float>(nullable: true),
                    qty_received = table.Column<float>(nullable: true),
                    saved = table.Column<string>(nullable: true),
                    created_at = table.Column<DateTime>(nullable: true),
                    updated_at = table.Column<DateTime>(nullable: true)
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

            migrationBuilder.CreateTable(
                name: "AdjustmentFG",
                columns: table => new
                {
                    id_adjustmentFG = table.Column<int>(nullable: false)
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
                    table.PrimaryKey("PK_AdjustmentFG", x => x.id_adjustmentFG);
                    table.ForeignKey(
                        name: "FK_AdjustmentFG_Master_BMI_bmi_code",
                        column: x => x.bmi_code,
                        principalTable: "Master_BMI",
                        principalColumn: "bmi_code",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AdjustmentFG_Pt_id_pt",
                        column: x => x.id_pt,
                        principalTable: "Pt",
                        principalColumn: "id_pt",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Production_input",
                columns: table => new
                {
                    id_productioninput = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    po = table.Column<string>(nullable: true),
                    date = table.Column<DateTime>(nullable: false),
                    id_pt = table.Column<string>(nullable: true),
                    raw_source = table.Column<string>(nullable: true),
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
                    table.ForeignKey(
                        name: "FK_Production_input_Pt_id_pt",
                        column: x => x.id_pt,
                        principalTable: "Pt",
                        principalColumn: "id_pt",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Production_output",
                columns: table => new
                {
                    id_productionoutput = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    po = table.Column<string>(nullable: true),
                    date = table.Column<DateTime>(nullable: false),
                    id_pt = table.Column<string>(nullable: true),
                    bmi_code = table.Column<string>(nullable: true),
                    qty = table.Column<float>(nullable: false),
                    raw_source = table.Column<string>(nullable: true),
                    saved = table.Column<string>(nullable: true),
                    created_at = table.Column<DateTime>(nullable: true),
                    updated_at = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Production_output", x => x.id_productionoutput);
                    table.ForeignKey(
                        name: "FK_Production_output_Master_BMI_bmi_code",
                        column: x => x.bmi_code,
                        principalTable: "Master_BMI",
                        principalColumn: "bmi_code",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Production_output_Pt_id_pt",
                        column: x => x.id_pt,
                        principalTable: "Pt",
                        principalColumn: "id_pt",
                        onDelete: ReferentialAction.Restrict);
                });

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
                    to_bmi_code = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Repack", x => x.id_repack);
                    table.ForeignKey(
                        name: "FK_Repack_Master_BMI_from_bmi_code",
                        column: x => x.from_bmi_code,
                        principalTable: "Master_BMI",
                        principalColumn: "bmi_code",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Repack_Pt_from_pt",
                        column: x => x.from_pt,
                        principalTable: "Pt",
                        principalColumn: "id_pt",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Repack_Master_BMI_to_bmi_code",
                        column: x => x.to_bmi_code,
                        principalTable: "Master_BMI",
                        principalColumn: "bmi_code",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Repack_Pt_to_pt",
                        column: x => x.to_pt,
                        principalTable: "Pt",
                        principalColumn: "id_pt",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Shipment_detail",
                columns: table => new
                {
                    id_shipment_detail = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    id_shipment = table.Column<int>(nullable: false),
                    bmi_code = table.Column<string>(nullable: true),
                    raw_source = table.Column<string>(nullable: true),
                    reff = table.Column<string>(nullable: true),
                    batch = table.Column<string>(nullable: true),
                    qty = table.Column<int>(nullable: false),
                    index = table.Column<float>(nullable: false),
                    saved = table.Column<string>(nullable: true),
                    created_at = table.Column<DateTime>(nullable: false),
                    updated_at = table.Column<DateTime>(nullable: false)
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
                        name: "FK_Shipment_detail_Shipment_id_shipment",
                        column: x => x.id_shipment,
                        principalTable: "Shipment",
                        principalColumn: "id_shipment",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AdjustmentFG_bmi_code",
                table: "AdjustmentFG",
                column: "bmi_code");

            migrationBuilder.CreateIndex(
                name: "IX_AdjustmentFG_id_pt",
                table: "AdjustmentFG",
                column: "id_pt");

            migrationBuilder.CreateIndex(
                name: "IX_AdjustmentRaw_sap_code",
                table: "AdjustmentRaw",
                column: "sap_code");

            migrationBuilder.CreateIndex(
                name: "IX_Fg_sap_code",
                table: "Fg",
                column: "sap_code");

            migrationBuilder.CreateIndex(
                name: "IX_Production_input_bmi_code",
                table: "Production_input",
                column: "bmi_code");

            migrationBuilder.CreateIndex(
                name: "IX_Production_input_id_pt",
                table: "Production_input",
                column: "id_pt");

            migrationBuilder.CreateIndex(
                name: "IX_Production_output_bmi_code",
                table: "Production_output",
                column: "bmi_code");

            migrationBuilder.CreateIndex(
                name: "IX_Production_output_id_pt",
                table: "Production_output",
                column: "id_pt");

            migrationBuilder.CreateIndex(
                name: "IX_Repack_from_bmi_code",
                table: "Repack",
                column: "from_bmi_code");

            migrationBuilder.CreateIndex(
                name: "IX_Repack_from_pt",
                table: "Repack",
                column: "from_pt");

            migrationBuilder.CreateIndex(
                name: "IX_Repack_to_bmi_code",
                table: "Repack",
                column: "to_bmi_code");

            migrationBuilder.CreateIndex(
                name: "IX_Repack_to_pt",
                table: "Repack",
                column: "to_pt");

            migrationBuilder.CreateIndex(
                name: "IX_Rm_sap_code",
                table: "Rm",
                column: "sap_code");

            migrationBuilder.CreateIndex(
                name: "IX_Shipment_detail_bmi_code",
                table: "Shipment_detail",
                column: "bmi_code");

            migrationBuilder.CreateIndex(
                name: "IX_Shipment_detail_id_shipment",
                table: "Shipment_detail",
                column: "id_shipment");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AdjustmentFG");

            migrationBuilder.DropTable(
                name: "AdjustmentRaw");

            migrationBuilder.DropTable(
                name: "Fg");

            migrationBuilder.DropTable(
                name: "Production_input");

            migrationBuilder.DropTable(
                name: "Production_output");

            migrationBuilder.DropTable(
                name: "Repack");

            migrationBuilder.DropTable(
                name: "Rm");

            migrationBuilder.DropTable(
                name: "Shipment_detail");

            migrationBuilder.DropTable(
                name: "User");

            migrationBuilder.DropTable(
                name: "Pt");

            migrationBuilder.DropTable(
                name: "Master_data");

            migrationBuilder.DropTable(
                name: "Master_BMI");

            migrationBuilder.DropTable(
                name: "Shipment");
        }
    }
}
