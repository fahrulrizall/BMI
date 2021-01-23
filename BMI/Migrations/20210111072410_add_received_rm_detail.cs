using Microsoft.EntityFrameworkCore.Migrations;

namespace BMI.Migrations
{
    public partial class add_received_rm_detail : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "saved",
                table: "Rm_detail");

            migrationBuilder.AddColumn<float>(
                name: "landing_site_received",
                table: "Rm_detail",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "landing_site_received",
                table: "Rm_detail");

            migrationBuilder.AddColumn<string>(
                name: "saved",
                table: "Rm_detail",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
