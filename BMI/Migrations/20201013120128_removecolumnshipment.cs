using Microsoft.EntityFrameworkCore.Migrations;

namespace BMI.Migrations
{
    public partial class removecolumnshipment : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Shipment",
                table: "Shipment");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddPrimaryKey(
                name: "PK_Shipment",
                table: "Shipment",
                column: "id_shipment");
        }
    }
}
