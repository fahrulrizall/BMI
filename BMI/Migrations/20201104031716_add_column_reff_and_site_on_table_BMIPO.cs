using Microsoft.EntityFrameworkCore.Migrations;

namespace BMI.Migrations
{
    public partial class add_column_reff_and_site_on_table_BMIPO : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "landing_site",
                table: "Bmi_po",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "reff",
                table: "Bmi_po",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "landing_site",
                table: "Bmi_po");

            migrationBuilder.DropColumn(
                name: "reff",
                table: "Bmi_po");
        }
    }
}
