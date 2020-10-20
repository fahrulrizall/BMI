using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BMI.Migrations
{
    public partial class addtablemasterbmi : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Master_BMI",
                columns: table => new
                {
                    bmi_code = table.Column<string>(nullable: false),
                    sap_code = table.Column<string>(nullable: true),
                    description = table.Column<string>(nullable: true),
                    lbs = table.Column<float>(nullable: true),
                    index_category = table.Column<string>(nullable: true),
                    daily_category = table.Column<string>(nullable: true),
                    weekly_category = table.Column<string>(nullable: true),
                    index = table.Column<float>(nullable: true),
                    index_lb = table.Column<float>(nullable: true),
                    index_cs = table.Column<float>(nullable: true),
                    zafc_kg = table.Column<float>(nullable: true),
                    zafc_cs = table.Column<float>(nullable: true),
                    created_at = table.Column<DateTime>(nullable: true),
                    updated_at = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Master_BMI", x => x.bmi_code);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Master_BMI");
        }
    }
}
