using Microsoft.EntityFrameworkCore.Migrations;

namespace BMI.Migrations
{
    public partial class change_qty : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<double>(
                name: "qty",
                table: "Repack",
                nullable: false,
                oldClrType: typeof(float),
                oldType: "real");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<float>(
                name: "qty",
                table: "Repack",
                type: "real",
                nullable: false,
                oldClrType: typeof(double));
        }
    }
}
