using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BMI.Migrations
{
    public partial class add_rm_detail_table : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "container",
                table: "Rm_detail");

            migrationBuilder.DropColumn(
                name: "eta",
                table: "Rm_detail");

            migrationBuilder.DropColumn(
                name: "etd",
                table: "Rm_detail");

            migrationBuilder.DropColumn(
                name: "status",
                table: "Rm_detail");

            migrationBuilder.AlterColumn<string>(
                name: "raw_source",
                table: "Rm_detail",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "RmModel",
                columns: table => new
                {
                    raw_source = table.Column<string>(nullable: false),
                    etd = table.Column<DateTime>(nullable: false),
                    eta = table.Column<DateTime>(nullable: false),
                    container = table.Column<string>(nullable: true),
                    status = table.Column<string>(nullable: true),
                    saved = table.Column<string>(nullable: true),
                    created_at = table.Column<DateTime>(nullable: true),
                    updated_at = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RmModel", x => x.raw_source);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Rm_detail_raw_source",
                table: "Rm_detail",
                column: "raw_source");

            migrationBuilder.AddForeignKey(
                name: "FK_Rm_detail_RmModel_raw_source",
                table: "Rm_detail",
                column: "raw_source",
                principalTable: "RmModel",
                principalColumn: "raw_source",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Rm_detail_RmModel_raw_source",
                table: "Rm_detail");

            migrationBuilder.DropTable(
                name: "RmModel");

            migrationBuilder.DropIndex(
                name: "IX_Rm_detail_raw_source",
                table: "Rm_detail");

            migrationBuilder.AlterColumn<string>(
                name: "raw_source",
                table: "Rm_detail",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "container",
                table: "Rm_detail",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "eta",
                table: "Rm_detail",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "etd",
                table: "Rm_detail",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "status",
                table: "Rm_detail",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
