using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BMI.Migrations
{
    public partial class addrmtodatabase : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Raw");

            migrationBuilder.CreateTable(
                name: "Rm",
                columns: table => new
                {
                    id_raw = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    etd = table.Column<DateTime>(nullable: false),
                    eta = table.Column<DateTime>(nullable: false),
                    container = table.Column<string>(nullable: true),
                    reff = table.Column<string>(nullable: true),
                    status = table.Column<string>(nullable: true),
                    landing_site = table.Column<string>(nullable: true),
                    sap_code = table.Column<string>(nullable: false),
                    cases = table.Column<int>(nullable: false),
                    uom = table.Column<string>(nullable: true),
                    usd_price = table.Column<float>(nullable: false),
                    ex_rate = table.Column<float>(nullable: false),
                    qty_pl = table.Column<float>(nullable: false),
                    qty_received = table.Column<float>(nullable: false),
                    saved = table.Column<string>(nullable: true),
                    created_at = table.Column<DateTime>(nullable: false),
                    updated_at = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rm", x => x.id_raw);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Rm");

            migrationBuilder.CreateTable(
                name: "Raw",
                columns: table => new
                {
                    id_raw = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    cases = table.Column<int>(type: "int", nullable: false),
                    container = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: false),
                    eta = table.Column<DateTime>(type: "datetime2", nullable: false),
                    etd = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ex_rate = table.Column<float>(type: "real", nullable: false),
                    landing_site = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    qty_pl = table.Column<float>(type: "real", nullable: false),
                    qty_received = table.Column<float>(type: "real", nullable: false),
                    reff = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    sap_code = table.Column<string>(type: "nvarchar(1)", nullable: false),
                    saved = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    status = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    uom = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    updated_at = table.Column<DateTime>(type: "datetime2", nullable: false),
                    usd_price = table.Column<float>(type: "real", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Raw", x => x.id_raw);
                });
        }
    }
}
