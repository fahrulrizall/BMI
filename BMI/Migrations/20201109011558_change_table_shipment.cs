using Microsoft.EntityFrameworkCore.Migrations;

namespace BMI.Migrations
{
    public partial class change_table_shipment : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Shipment_detail_Shipment_id_ship",
                table: "Shipment_detail");

            migrationBuilder.DropIndex(
                name: "IX_Shipment_detail_id_ship",
                table: "Shipment_detail");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Shipment",
                table: "Shipment");

            migrationBuilder.DropColumn(
                name: "id_ship",
                table: "Shipment_detail");

            migrationBuilder.DropColumn(
                name: "id_ship",
                table: "Shipment");

            migrationBuilder.AddColumn<int>(
                name: "id_shipment",
                table: "Shipment_detail",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "id_shipment",
                table: "Shipment",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Shipment",
                table: "Shipment",
                column: "id_shipment");

            migrationBuilder.CreateIndex(
                name: "IX_Shipment_detail_id_shipment",
                table: "Shipment_detail",
                column: "id_shipment");

            migrationBuilder.AddForeignKey(
                name: "FK_Shipment_detail_Shipment_id_shipment",
                table: "Shipment_detail",
                column: "id_shipment",
                principalTable: "Shipment",
                principalColumn: "id_shipment",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Shipment_detail_Shipment_id_shipment",
                table: "Shipment_detail");

            migrationBuilder.DropIndex(
                name: "IX_Shipment_detail_id_shipment",
                table: "Shipment_detail");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Shipment",
                table: "Shipment");

            migrationBuilder.DropColumn(
                name: "id_shipment",
                table: "Shipment_detail");

            migrationBuilder.DropColumn(
                name: "id_shipment",
                table: "Shipment");

            migrationBuilder.AddColumn<int>(
                name: "id_ship",
                table: "Shipment_detail",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "id_ship",
                table: "Shipment",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Shipment",
                table: "Shipment",
                column: "id_ship");

            migrationBuilder.CreateIndex(
                name: "IX_Shipment_detail_id_ship",
                table: "Shipment_detail",
                column: "id_ship");

            migrationBuilder.AddForeignKey(
                name: "FK_Shipment_detail_Shipment_id_ship",
                table: "Shipment_detail",
                column: "id_ship",
                principalTable: "Shipment",
                principalColumn: "id_ship",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
