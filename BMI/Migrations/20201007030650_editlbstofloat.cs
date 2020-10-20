using Microsoft.EntityFrameworkCore.Migrations;

namespace BMI.Migrations
{
    public partial class editlbstofloat : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<float>(
                name: "lbs",
                table: "Master_data",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "lbs",
                table: "Master_data",
                type: "int",
                nullable: false,
                oldClrType: typeof(float));
        }
    }
}
