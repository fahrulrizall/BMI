using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BMI.Migrations
{
    public partial class addshipmenttable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Shipment",
                columns: table => new
                {
                    id_shipment = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    etd = table.Column<DateTime>(nullable: false),
                    eta = table.Column<DateTime>(nullable: false),
                    destination = table.Column<string>(nullable: true),
                    po = table.Column<int>(nullable: false),
                    saved = table.Column<string>(nullable: true),
                    created_at = table.Column<DateTime>(nullable: true),
                    updated_at = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Shipment", x => x.id_shipment);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Shipment");
        }
    }
}
