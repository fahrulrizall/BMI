using Microsoft.EntityFrameworkCore.Migrations;

namespace BMI.Migrations
{
    public partial class removecolumnshipmentpart3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "id_shipment",
                table: "Shipment"
                );
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
