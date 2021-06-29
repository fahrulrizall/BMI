using Microsoft.EntityFrameworkCore.Migrations;

namespace BMI.Migrations
{
    public partial class delete_version_model_PT : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Version",
                table: "CostAnalyst");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Version",
                table: "CostAnalyst",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
