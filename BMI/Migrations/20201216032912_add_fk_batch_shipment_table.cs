using Microsoft.EntityFrameworkCore.Migrations;

namespace BMI.Migrations
{
    public partial class add_fk_batch_shipment_table : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "batch",
                table: "Shipment",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Shipment_batch",
                table: "Shipment",
                column: "batch");

            migrationBuilder.AddForeignKey(
                name: "FK_Shipment_PO_batch",
                table: "Shipment",
                column: "batch",
                principalTable: "PO",
                principalColumn: "po",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Shipment_PO_batch",
                table: "Shipment");

            migrationBuilder.DropIndex(
                name: "IX_Shipment_batch",
                table: "Shipment");

            migrationBuilder.AlterColumn<string>(
                name: "batch",
                table: "Shipment",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);
        }
    }
}
