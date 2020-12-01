using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BMI.Migrations
{
    public partial class add_coloumn_table_shipment : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "container",
                table: "Shipment",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "document_date",
                table: "Shipment",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "fda_no",
                table: "Shipment",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "house_bol",
                table: "Shipment",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "inv_no",
                table: "Shipment",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "master_bol",
                table: "Shipment",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ocean_carrier",
                table: "Shipment",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "port_loading",
                table: "Shipment",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "port_receipt",
                table: "Shipment",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "seal_no",
                table: "Shipment",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "vessel_name",
                table: "Shipment",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "voyage_no",
                table: "Shipment",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "container",
                table: "Shipment");

            migrationBuilder.DropColumn(
                name: "document_date",
                table: "Shipment");

            migrationBuilder.DropColumn(
                name: "fda_no",
                table: "Shipment");

            migrationBuilder.DropColumn(
                name: "house_bol",
                table: "Shipment");

            migrationBuilder.DropColumn(
                name: "inv_no",
                table: "Shipment");

            migrationBuilder.DropColumn(
                name: "master_bol",
                table: "Shipment");

            migrationBuilder.DropColumn(
                name: "ocean_carrier",
                table: "Shipment");

            migrationBuilder.DropColumn(
                name: "port_loading",
                table: "Shipment");

            migrationBuilder.DropColumn(
                name: "port_receipt",
                table: "Shipment");

            migrationBuilder.DropColumn(
                name: "seal_no",
                table: "Shipment");

            migrationBuilder.DropColumn(
                name: "vessel_name",
                table: "Shipment");

            migrationBuilder.DropColumn(
                name: "voyage_no",
                table: "Shipment");
        }
    }
}
