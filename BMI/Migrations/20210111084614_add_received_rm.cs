using Microsoft.EntityFrameworkCore.Migrations;

namespace BMI.Migrations
{
    public partial class add_received_rm : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "saved",
                table: "Rm");

            migrationBuilder.DropColumn(
                name: "saved",
                table: "PO");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "saved",
                table: "Rm",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "saved",
                table: "PO",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
