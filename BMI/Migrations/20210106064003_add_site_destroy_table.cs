using Microsoft.EntityFrameworkCore.Migrations;

namespace BMI.Migrations
{
    public partial class add_site_destroy_table : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "landing_site",
                table: "AdjustmentFG",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "landing_site",
                table: "AdjustmentFG");
        }
    }
}
