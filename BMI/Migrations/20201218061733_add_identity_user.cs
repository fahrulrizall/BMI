using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BMI.Migrations
{
    public partial class add_identity_user : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "created_by",
                table: "Rm_detail",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "updated_by",
                table: "Rm_detail",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "created_by",
                table: "Rm",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "updated_by",
                table: "Rm",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "created_by",
                table: "Production_output",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "updated_by",
                table: "Production_output",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "created_by",
                table: "Production_input",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "updated_by",
                table: "Production_input",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "created_by",
                table: "PO",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "updated_by",
                table: "PO",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "created_at",
                table: "Pending",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "created_by",
                table: "Pending",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "updated_at",
                table: "Pending",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "updated_by",
                table: "Pending",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "created_by",
                table: "Master_data",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "updated_by",
                table: "Master_data",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "created_by",
                table: "Master_BMI",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "updated_by",
                table: "Master_BMI",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "created_by",
                table: "Fg",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "updated_by",
                table: "Fg",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "created_at",
                table: "AdjustmentRaw",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "created_by",
                table: "AdjustmentRaw",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "updated_at",
                table: "AdjustmentRaw",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "updated_by",
                table: "AdjustmentRaw",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "created_at",
                table: "AdjustmentFG",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "created_by",
                table: "AdjustmentFG",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "updated_at",
                table: "AdjustmentFG",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "updated_by",
                table: "AdjustmentFG",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "created_by",
                table: "Rm_detail");

            migrationBuilder.DropColumn(
                name: "updated_by",
                table: "Rm_detail");

            migrationBuilder.DropColumn(
                name: "created_by",
                table: "Rm");

            migrationBuilder.DropColumn(
                name: "updated_by",
                table: "Rm");

            migrationBuilder.DropColumn(
                name: "created_by",
                table: "Production_output");

            migrationBuilder.DropColumn(
                name: "updated_by",
                table: "Production_output");

            migrationBuilder.DropColumn(
                name: "created_by",
                table: "Production_input");

            migrationBuilder.DropColumn(
                name: "updated_by",
                table: "Production_input");

            migrationBuilder.DropColumn(
                name: "created_by",
                table: "PO");

            migrationBuilder.DropColumn(
                name: "updated_by",
                table: "PO");

            migrationBuilder.DropColumn(
                name: "created_at",
                table: "Pending");

            migrationBuilder.DropColumn(
                name: "created_by",
                table: "Pending");

            migrationBuilder.DropColumn(
                name: "updated_at",
                table: "Pending");

            migrationBuilder.DropColumn(
                name: "updated_by",
                table: "Pending");

            migrationBuilder.DropColumn(
                name: "created_by",
                table: "Master_data");

            migrationBuilder.DropColumn(
                name: "updated_by",
                table: "Master_data");

            migrationBuilder.DropColumn(
                name: "created_by",
                table: "Master_BMI");

            migrationBuilder.DropColumn(
                name: "updated_by",
                table: "Master_BMI");

            migrationBuilder.DropColumn(
                name: "created_by",
                table: "Fg");

            migrationBuilder.DropColumn(
                name: "updated_by",
                table: "Fg");

            migrationBuilder.DropColumn(
                name: "created_at",
                table: "AdjustmentRaw");

            migrationBuilder.DropColumn(
                name: "created_by",
                table: "AdjustmentRaw");

            migrationBuilder.DropColumn(
                name: "updated_at",
                table: "AdjustmentRaw");

            migrationBuilder.DropColumn(
                name: "updated_by",
                table: "AdjustmentRaw");

            migrationBuilder.DropColumn(
                name: "created_at",
                table: "AdjustmentFG");

            migrationBuilder.DropColumn(
                name: "created_by",
                table: "AdjustmentFG");

            migrationBuilder.DropColumn(
                name: "updated_at",
                table: "AdjustmentFG");

            migrationBuilder.DropColumn(
                name: "updated_by",
                table: "AdjustmentFG");
        }
    }
}
