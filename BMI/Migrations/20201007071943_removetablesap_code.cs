using Microsoft.EntityFrameworkCore.Migrations;

namespace BMI.Migrations
{
    public partial class removetablesap_code : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "sap_code",
                table: "Fg");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            
        }
    }
}
