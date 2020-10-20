using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BMI.Migrations
{
    public partial class addtableshipmentdetail : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Shipment_detail",
                columns: table => new
                {
                    id_shipment_detail = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    id_ship = table.Column<int>(nullable: false),
                    bmi_code = table.Column<string>(nullable: true),
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
                        name: "FK_Shipment_detail_Shipment_id_ship",
                        column: x => x.id_ship,
                        principalTable: "Shipment",
                        principalColumn: "id_ship",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Shipment_detail_bmi_code",
                table: "Shipment_detail",
                column: "bmi_code");

            migrationBuilder.CreateIndex(
                name: "IX_Shipment_detail_id_ship",
                table: "Shipment_detail",
                column: "id_ship");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Shipment_detail");
        }
    }
}
