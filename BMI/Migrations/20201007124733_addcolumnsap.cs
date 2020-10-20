using Microsoft.EntityFrameworkCore.Migrations;

namespace BMI.Migrations
{
    public partial class addcolumnsap : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "sap_code",
                table: "Fg",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
