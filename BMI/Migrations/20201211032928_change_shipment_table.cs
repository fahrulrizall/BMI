using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BMI.Migrations
{
    public partial class change_shipment_table : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Shipment_detail");

            migrationBuilder.DropColumn(
                name: "saved",
                table: "Shipment");

            migrationBuilder.AddColumn<string>(
                name: "created_by",
                table: "Shipment",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "pdc",
                table: "Shipment",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "qty",
                table: "Shipment",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "raw_source",
                table: "Shipment",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "updated_by",
                table: "Shipment",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "created_by",
                table: "Shipment");

            migrationBuilder.DropColumn(
                name: "pdc",
                table: "Shipment");

            migrationBuilder.DropColumn(
                name: "qty",
                table: "Shipment");

            migrationBuilder.DropColumn(
                name: "raw_source",
                table: "Shipment");

            migrationBuilder.DropColumn(
                name: "updated_by",
                table: "Shipment");

            migrationBuilder.AddColumn<string>(
                name: "saved",
                table: "Shipment",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Shipment_detail",
                columns: table => new
                {
                    id_shipment_detail = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CS_location = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: true),
                    created_by = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    id_shipment = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    landing_site = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    pdc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    qty = table.Column<int>(type: "int", nullable: false),
                    raw_source = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    updated_at = table.Column<DateTime>(type: "datetime2", nullable: true),
                    updated_by = table.Column<string>(type: "nvarchar(max)", nullable: true)
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
                name: "IX_Shipment_detail_id_shipment",
                table: "Shipment_detail",
                column: "id_shipment");
        }
    }
}
