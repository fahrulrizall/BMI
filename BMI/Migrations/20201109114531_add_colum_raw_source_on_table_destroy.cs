using Microsoft.EntityFrameworkCore.Migrations;

namespace BMI.Migrations
{
    public partial class add_colum_raw_source_on_table_destroy : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "raw_source",
                table: "Shipment_detail",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "raw_source",
                table: "Destroy",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "raw_source",
                table: "Shipment_detail");

            migrationBuilder.DropColumn(
                name: "raw_source",
                table: "Destroy");
        }
    }
}
