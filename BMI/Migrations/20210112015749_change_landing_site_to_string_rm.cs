using Microsoft.EntityFrameworkCore.Migrations;

namespace BMI.Migrations
{
    public partial class change_landing_site_to_string_rm : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "landing_site_received",
                table: "Rm_detail",
                nullable: true,
                oldClrType: typeof(float),
                oldType: "real",
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<float>(
                name: "landing_site_received",
                table: "Rm_detail",
                type: "real",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);
        }
    }
}
