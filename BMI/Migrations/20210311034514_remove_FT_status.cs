using Microsoft.EntityFrameworkCore.Migrations;

namespace BMI.Migrations
{
    public partial class remove_FT_status : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "fairtrade_status",
                table: "Rm_detail");

            migrationBuilder.DropColumn(
                name: "fairtrade_status",
                table: "Production_input");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "fairtrade_status",
                table: "Rm_detail",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "fairtrade_status",
                table: "Production_input",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
