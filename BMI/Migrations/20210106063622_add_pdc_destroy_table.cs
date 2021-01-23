using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BMI.Migrations
{
    public partial class add_pdc_destroy_table : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "production_date",
                table: "AdjustmentFG",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "production_date",
                table: "AdjustmentFG");
        }
    }
}
