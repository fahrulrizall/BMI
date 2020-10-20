using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BMI.Migrations
{
    public partial class addrawtodatabase : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Raw",
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
                    table.PrimaryKey("PK_Raw", x => x.id_raw);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Raw");
        }
    }
}
