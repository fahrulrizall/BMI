using Microsoft.EntityFrameworkCore.Migrations;

namespace BMI.Migrations
{
    public partial class change_pt_to_no_pt_on_table_production : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "pt",
                table: "Production_output");

            migrationBuilder.DropColumn(
                name: "pt",
                table: "Production_input");

            migrationBuilder.DropColumn(
                name: "reff",
                table: "Production_input");

            migrationBuilder.AddColumn<int>(
                name: "id_pt",
                table: "Production_output",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "id_pt",
                table: "Production_input",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "raw_source",
                table: "Production_input",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "id_pt",
                table: "Production_output");

            migrationBuilder.DropColumn(
                name: "id_pt",
                table: "Production_input");

            migrationBuilder.DropColumn(
                name: "raw_source",
                table: "Production_input");

            migrationBuilder.AddColumn<int>(
                name: "pt",
                table: "Production_output",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "pt",
                table: "Production_input",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "reff",
                table: "Production_input",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
