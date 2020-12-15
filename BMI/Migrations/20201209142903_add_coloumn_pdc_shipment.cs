using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BMI.Migrations
{
    public partial class add_coloumn_pdc_shipment : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "reff",
                table: "Shipment_detail");

            migrationBuilder.AddColumn<string>(
                name: "landing_site",
                table: "Shipment_detail",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "pdc",
                table: "Shipment_detail",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "landing_site",
                table: "Shipment_detail");

            migrationBuilder.DropColumn(
                name: "pdc",
                table: "Shipment_detail");

            migrationBuilder.AddColumn<string>(
                name: "reff",
                table: "Shipment_detail",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
