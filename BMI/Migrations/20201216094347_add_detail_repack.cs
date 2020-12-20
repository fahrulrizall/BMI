using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BMI.Migrations
{
    public partial class add_detail_repack : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "created_at",
                table: "Repack",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "created_by",
                table: "Repack",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "landing_site",
                table: "Repack",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "updated_at",
                table: "Repack",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "updated_by",
                table: "Repack",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "created_at",
                table: "Repack");

            migrationBuilder.DropColumn(
                name: "created_by",
                table: "Repack");

            migrationBuilder.DropColumn(
                name: "landing_site",
                table: "Repack");

            migrationBuilder.DropColumn(
                name: "updated_at",
                table: "Repack");

            migrationBuilder.DropColumn(
                name: "updated_by",
                table: "Repack");
        }
    }
}
