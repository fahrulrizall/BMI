using Microsoft.EntityFrameworkCore.Migrations;

namespace BMI.Migrations
{
    public partial class removecolumnshipmentpart4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "id_ship",
                table: "Shipment",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Shipment",
                table: "Shipment",
                column: "id_ship");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Shipment",
                table: "Shipment");

            migrationBuilder.DropColumn(
                name: "id_ship",
                table: "Shipment");
        }
    }
}
