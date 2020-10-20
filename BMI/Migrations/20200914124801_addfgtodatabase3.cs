using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BMI.Migrations
{
    public partial class addfgtodatabase3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Fg",
                columns: table => new
                {
                    id_fg = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    sap_code = table.Column<string>(nullable: false),
                    plant = table.Column<int>(nullable: false),
                    price_lbs = table.Column<float>(nullable: false),
                    std_price = table.Column<float>(nullable: false),
                    processing_fee = table.Column<float>(nullable: false),
                    created_at = table.Column<DateTime>(nullable: false),
                    updated_at = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Fg", x => x.id_fg);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Fg");
        }
    }
}
