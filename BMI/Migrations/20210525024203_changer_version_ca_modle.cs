using Microsoft.EntityFrameworkCore.Migrations;

namespace BMI.Migrations
{
    public partial class changer_version_ca_modle : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Price",
                table: "CostAnalyst");

            migrationBuilder.AlterColumn<string>(
                name: "Version",
                table: "CostAnalyst",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "Version",
                table: "CostAnalyst",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddColumn<float>(
                name: "Price",
                table: "CostAnalyst",
                type: "real",
                nullable: false,
                defaultValue: 0f);
        }
    }
}
