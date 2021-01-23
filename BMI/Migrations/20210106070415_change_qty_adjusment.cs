using Microsoft.EntityFrameworkCore.Migrations;

namespace BMI.Migrations
{
    public partial class change_qty_adjusment : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<double>(
                name: "qty",
                table: "AdjustmentFG",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "qty",
                table: "AdjustmentFG",
                type: "int",
                nullable: false,
                oldClrType: typeof(double));
        }
    }
}
