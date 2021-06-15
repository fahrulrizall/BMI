using Microsoft.EntityFrameworkCore.Migrations;

namespace BMI.Migrations
{
    public partial class add_material_in_PO_model : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Material",
                table: "CostAnalyst");

            migrationBuilder.AddColumn<string>(
                name: "Material",
                table: "PO",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Material",
                table: "PO");

            migrationBuilder.AddColumn<string>(
                name: "Material",
                table: "CostAnalyst",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
