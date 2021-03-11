using Microsoft.EntityFrameworkCore.Migrations;

namespace BMI.Migrations
{
    public partial class add_ft_status : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "fairtrade_status",
                table: "Rm_detail",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "fairtrade_status",
                table: "Production_output",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "fairtrade_status",
                table: "Production_input",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "fairtrade_status",
                table: "Rm_detail");

            migrationBuilder.DropColumn(
                name: "fairtrade_status",
                table: "Production_output");

            migrationBuilder.DropColumn(
                name: "fairtrade_status",
                table: "Production_input");
        }
    }
}
