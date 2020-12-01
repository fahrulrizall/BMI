using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BMI.Migrations
{
    public partial class change_table : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AdjustmentFG_Pt_id_pt",
                table: "AdjustmentFG");

            //migrationBuilder.DropForeignKey(
            //    name: "FK_Production_input_Pt_id_pt",
            //    table: "Production_input");

            //migrationBuilder.DropForeignKey(
            //    name: "FK_Production_output_Pt_id_pt",
            //    table: "Production_output");

            migrationBuilder.DropForeignKey(
                name: "FK_Repack_Pt_from_pt",
                table: "Repack");

            migrationBuilder.DropForeignKey(
                name: "FK_Repack_Pt_to_pt",
                table: "Repack");

            migrationBuilder.DropForeignKey(
                name: "FK_Shipment_detail_Master_BMI_bmi_code",
                table: "Shipment_detail");

            migrationBuilder.DropForeignKey(
                name: "FK_Shipment_detail_Shipment_id_shipment",
                table: "Shipment_detail");

            migrationBuilder.DropTable(
                name: "Pt");

            migrationBuilder.DropTable(
                name: "Shipment");

            migrationBuilder.DropIndex(
                name: "IX_Shipment_detail_bmi_code",
                table: "Shipment_detail");

            migrationBuilder.DropIndex(
                name: "IX_Shipment_detail_id_shipment",
                table: "Shipment_detail");

            migrationBuilder.DropIndex(
                name: "IX_Repack_from_pt",
                table: "Repack");

            migrationBuilder.DropIndex(
                name: "IX_Repack_to_pt",
                table: "Repack");

            migrationBuilder.DropIndex(
                name: "IX_Production_output_id_pt",
                table: "Production_output");

            migrationBuilder.DropIndex(
                name: "IX_Production_input_id_pt",
                table: "Production_input");

            migrationBuilder.DropIndex(
                name: "IX_AdjustmentFG_id_pt",
                table: "AdjustmentFG");

            migrationBuilder.DropColumn(
                name: "id_shipment",
                table: "Shipment_detail");

            migrationBuilder.AlterColumn<string>(
                name: "bmi_code",
                table: "Shipment_detail",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MasterBMIModelbmi_code",
                table: "Shipment_detail",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "to_pt",
                table: "Repack",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "from_pt",
                table: "Repack",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "id_pt",
                table: "Production_output",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "id_pt",
                table: "Production_input",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "id_pt",
                table: "AdjustmentFG",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "PO",
                columns: table => new
                {
                    po = table.Column<string>(nullable: false),
                    pt = table.Column<int>(nullable: false),
                    plant = table.Column<string>(nullable: true),
                    batch = table.Column<string>(nullable: true),
                    status = table.Column<string>(nullable: true),
                    etd = table.Column<DateTime>(nullable: false),
                    eta = table.Column<DateTime>(nullable: false),
                    destination = table.Column<string>(nullable: false),
                    container = table.Column<string>(nullable: true),
                    inv_no = table.Column<string>(nullable: true),
                    fda_no = table.Column<string>(nullable: true),
                    seal_no = table.Column<string>(nullable: true),
                    ocean_carrier = table.Column<string>(nullable: true),
                    document_date = table.Column<DateTime>(nullable: true),
                    vessel_name = table.Column<string>(nullable: true),
                    master_bol = table.Column<string>(nullable: true),
                    house_bol = table.Column<string>(nullable: true),
                    voyage_no = table.Column<string>(nullable: true),
                    port_loading = table.Column<string>(nullable: true),
                    port_receipt = table.Column<string>(nullable: true),
                    saved = table.Column<string>(nullable: true),
                    created_at = table.Column<DateTime>(nullable: true),
                    updated_at = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PO", x => x.po);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Shipment_detail_MasterBMIModelbmi_code",
                table: "Shipment_detail",
                column: "MasterBMIModelbmi_code");

            migrationBuilder.AddForeignKey(
                name: "FK_Shipment_detail_Master_BMI_MasterBMIModelbmi_code",
                table: "Shipment_detail",
                column: "MasterBMIModelbmi_code",
                principalTable: "Master_BMI",
                principalColumn: "bmi_code",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Shipment_detail_Master_BMI_MasterBMIModelbmi_code",
                table: "Shipment_detail");

            migrationBuilder.DropTable(
                name: "PO");

            migrationBuilder.DropIndex(
                name: "IX_Shipment_detail_MasterBMIModelbmi_code",
                table: "Shipment_detail");

            migrationBuilder.DropColumn(
                name: "MasterBMIModelbmi_code",
                table: "Shipment_detail");

            migrationBuilder.AlterColumn<string>(
                name: "bmi_code",
                table: "Shipment_detail",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "id_shipment",
                table: "Shipment_detail",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<string>(
                name: "to_pt",
                table: "Repack",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "from_pt",
                table: "Repack",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "id_pt",
                table: "Production_output",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "id_pt",
                table: "Production_input",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "id_pt",
                table: "AdjustmentFG",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "Pt",
                columns: table => new
                {
                    id_pt = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    batch = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    plant = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    po = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    pt = table.Column<int>(type: "int", nullable: false),
                    status = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pt", x => x.id_pt);
                });

            migrationBuilder.CreateTable(
                name: "Shipment",
                columns: table => new
                {
                    id_shipment = table.Column<int>(type: "int", nullable: false),
                    container = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: true),
                    destination = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    document_date = table.Column<DateTime>(type: "datetime2", nullable: true),
                    eta = table.Column<DateTime>(type: "datetime2", nullable: false),
                    etd = table.Column<DateTime>(type: "datetime2", nullable: false),
                    fda_no = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    house_bol = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    inv_no = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    master_bol = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ocean_carrier = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    po = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    port_loading = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    port_receipt = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    saved = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    seal_no = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    updated_at = table.Column<DateTime>(type: "datetime2", nullable: true),
                    vessel_name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    voyage_no = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Shipment", x => x.id_shipment);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Shipment_detail_bmi_code",
                table: "Shipment_detail",
                column: "bmi_code");

            migrationBuilder.CreateIndex(
                name: "IX_Shipment_detail_id_shipment",
                table: "Shipment_detail",
                column: "id_shipment");

            migrationBuilder.CreateIndex(
                name: "IX_Repack_from_pt",
                table: "Repack",
                column: "from_pt");

            migrationBuilder.CreateIndex(
                name: "IX_Repack_to_pt",
                table: "Repack",
                column: "to_pt");

            migrationBuilder.CreateIndex(
                name: "IX_Production_output_id_pt",
                table: "Production_output",
                column: "id_pt");

            migrationBuilder.CreateIndex(
                name: "IX_Production_input_id_pt",
                table: "Production_input",
                column: "id_pt");

            migrationBuilder.CreateIndex(
                name: "IX_AdjustmentFG_id_pt",
                table: "AdjustmentFG",
                column: "id_pt");

            migrationBuilder.AddForeignKey(
                name: "FK_AdjustmentFG_Pt_id_pt",
                table: "AdjustmentFG",
                column: "id_pt",
                principalTable: "Pt",
                principalColumn: "id_pt",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Production_input_Pt_id_pt",
                table: "Production_input",
                column: "id_pt",
                principalTable: "Pt",
                principalColumn: "id_pt",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Production_output_Pt_id_pt",
                table: "Production_output",
                column: "id_pt",
                principalTable: "Pt",
                principalColumn: "id_pt",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Repack_Pt_from_pt",
                table: "Repack",
                column: "from_pt",
                principalTable: "Pt",
                principalColumn: "id_pt",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Repack_Pt_to_pt",
                table: "Repack",
                column: "to_pt",
                principalTable: "Pt",
                principalColumn: "id_pt",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Shipment_detail_Master_BMI_bmi_code",
                table: "Shipment_detail",
                column: "bmi_code",
                principalTable: "Master_BMI",
                principalColumn: "bmi_code",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Shipment_detail_Shipment_id_shipment",
                table: "Shipment_detail",
                column: "id_shipment",
                principalTable: "Shipment",
                principalColumn: "id_shipment",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
