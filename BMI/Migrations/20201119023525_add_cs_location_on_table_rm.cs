using Microsoft.EntityFrameworkCore.Migrations;

namespace BMI.Migrations
{
    public partial class add_cs_location_on_table_rm : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CS_location",
                table: "Rm",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "landing_site",
                table: "Production_output",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CS_location",
                table: "Rm");

            migrationBuilder.DropColumn(
                name: "landing_site",
                table: "Production_output");
        }
    }
}
