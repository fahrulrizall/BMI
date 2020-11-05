using Microsoft.EntityFrameworkCore.Migrations;

namespace BMI.Migrations
{
    public partial class remove_column_reff_on_table_BMIPO : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "reff",
                table: "Bmi_po");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "reff",
                table: "Bmi_po",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
