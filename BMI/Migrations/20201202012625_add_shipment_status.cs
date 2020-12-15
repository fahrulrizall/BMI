using Microsoft.EntityFrameworkCore.Migrations;

namespace BMI.Migrations
{
    public partial class add_shipment_status : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "status",
                table: "PO");

            migrationBuilder.AddColumn<string>(
                name: "pt_status",
                table: "PO",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "shipment_status",
                table: "PO",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "pt_status",
                table: "PO");

            migrationBuilder.DropColumn(
                name: "shipment_status",
                table: "PO");

            migrationBuilder.AddColumn<string>(
                name: "status",
                table: "PO",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
