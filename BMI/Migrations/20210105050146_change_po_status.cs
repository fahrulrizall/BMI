using Microsoft.EntityFrameworkCore.Migrations;

namespace BMI.Migrations
{
    public partial class change_po_status : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "saved",
                table: "Production_output");

            migrationBuilder.DropColumn(
                name: "saved",
                table: "Production_input");

            migrationBuilder.DropColumn(
                name: "shipment_status",
                table: "PO");

            migrationBuilder.AddColumn<string>(
                name: "po_status",
                table: "PO",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "po_status",
                table: "PO");

            migrationBuilder.AddColumn<string>(
                name: "saved",
                table: "Production_output",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "saved",
                table: "Production_input",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "shipment_status",
                table: "PO",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
