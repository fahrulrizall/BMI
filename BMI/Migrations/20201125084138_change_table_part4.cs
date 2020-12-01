using Microsoft.EntityFrameworkCore.Migrations;

namespace BMI.Migrations
{
    public partial class change_table_part4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AdjustmentFG_PO_POModelpo",
                table: "AdjustmentFG");

            migrationBuilder.DropForeignKey(
                name: "FK_Production_input_PO_POModelpo",
                table: "Production_input");

            migrationBuilder.DropForeignKey(
                name: "FK_Production_output_PO_POModelpo",
                table: "Production_output");

            migrationBuilder.DropForeignKey(
                name: "FK_Repack_PO_fromPOModelpo",
                table: "Repack");

            migrationBuilder.DropForeignKey(
                name: "FK_Repack_PO_toPOModelpo",
                table: "Repack");

            migrationBuilder.DropForeignKey(
                name: "FK_Shipment_detail_PO_POModelpo",
                table: "Shipment_detail");

            migrationBuilder.DropIndex(
                name: "IX_Shipment_detail_POModelpo",
                table: "Shipment_detail");

            migrationBuilder.DropIndex(
                name: "IX_Repack_fromPOModelpo",
                table: "Repack");

            migrationBuilder.DropIndex(
                name: "IX_Repack_toPOModelpo",
                table: "Repack");

            migrationBuilder.DropIndex(
                name: "IX_Production_output_POModelpo",
                table: "Production_output");

            migrationBuilder.DropIndex(
                name: "IX_Production_input_POModelpo",
                table: "Production_input");

            migrationBuilder.DropIndex(
                name: "IX_AdjustmentFG_POModelpo",
                table: "AdjustmentFG");

            migrationBuilder.DropColumn(
                name: "POModelpo",
                table: "Shipment_detail");

            migrationBuilder.DropColumn(
                name: "fromPOModelpo",
                table: "Repack");

            migrationBuilder.DropColumn(
                name: "toPOModelpo",
                table: "Repack");

            migrationBuilder.DropColumn(
                name: "POModelpo",
                table: "Production_output");

            migrationBuilder.DropColumn(
                name: "POModelpo",
                table: "Production_input");

            migrationBuilder.DropColumn(
                name: "POModelpo",
                table: "AdjustmentFG");

            migrationBuilder.AlterColumn<string>(
                name: "po",
                table: "Shipment_detail",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "to_po",
                table: "Repack",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "from_po",
                table: "Repack",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "po",
                table: "Production_output",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "po",
                table: "Production_input",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "po",
                table: "AdjustmentFG",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Shipment_detail_po",
                table: "Shipment_detail",
                column: "po");

            migrationBuilder.CreateIndex(
                name: "IX_Repack_from_po",
                table: "Repack",
                column: "from_po");

            migrationBuilder.CreateIndex(
                name: "IX_Repack_to_po",
                table: "Repack",
                column: "to_po");

            migrationBuilder.CreateIndex(
                name: "IX_Production_output_po",
                table: "Production_output",
                column: "po");

            migrationBuilder.CreateIndex(
                name: "IX_Production_input_po",
                table: "Production_input",
                column: "po");

            migrationBuilder.CreateIndex(
                name: "IX_AdjustmentFG_po",
                table: "AdjustmentFG",
                column: "po");

            migrationBuilder.AddForeignKey(
                name: "FK_AdjustmentFG_PO_po",
                table: "AdjustmentFG",
                column: "po",
                principalTable: "PO",
                principalColumn: "po",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Production_input_PO_po",
                table: "Production_input",
                column: "po",
                principalTable: "PO",
                principalColumn: "po",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Production_output_PO_po",
                table: "Production_output",
                column: "po",
                principalTable: "PO",
                principalColumn: "po",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Repack_PO_from_po",
                table: "Repack",
                column: "from_po",
                principalTable: "PO",
                principalColumn: "po",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Repack_PO_to_po",
                table: "Repack",
                column: "to_po",
                principalTable: "PO",
                principalColumn: "po",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Shipment_detail_PO_po",
                table: "Shipment_detail",
                column: "po",
                principalTable: "PO",
                principalColumn: "po",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AdjustmentFG_PO_po",
                table: "AdjustmentFG");

            migrationBuilder.DropForeignKey(
                name: "FK_Production_input_PO_po",
                table: "Production_input");

            migrationBuilder.DropForeignKey(
                name: "FK_Production_output_PO_po",
                table: "Production_output");

            migrationBuilder.DropForeignKey(
                name: "FK_Repack_PO_from_po",
                table: "Repack");

            migrationBuilder.DropForeignKey(
                name: "FK_Repack_PO_to_po",
                table: "Repack");

            migrationBuilder.DropForeignKey(
                name: "FK_Shipment_detail_PO_po",
                table: "Shipment_detail");

            migrationBuilder.DropIndex(
                name: "IX_Shipment_detail_po",
                table: "Shipment_detail");

            migrationBuilder.DropIndex(
                name: "IX_Repack_from_po",
                table: "Repack");

            migrationBuilder.DropIndex(
                name: "IX_Repack_to_po",
                table: "Repack");

            migrationBuilder.DropIndex(
                name: "IX_Production_output_po",
                table: "Production_output");

            migrationBuilder.DropIndex(
                name: "IX_Production_input_po",
                table: "Production_input");

            migrationBuilder.DropIndex(
                name: "IX_AdjustmentFG_po",
                table: "AdjustmentFG");

            migrationBuilder.AlterColumn<string>(
                name: "po",
                table: "Shipment_detail",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "POModelpo",
                table: "Shipment_detail",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "to_po",
                table: "Repack",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "from_po",
                table: "Repack",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "fromPOModelpo",
                table: "Repack",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "toPOModelpo",
                table: "Repack",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "po",
                table: "Production_output",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "POModelpo",
                table: "Production_output",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "po",
                table: "Production_input",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "POModelpo",
                table: "Production_input",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "po",
                table: "AdjustmentFG",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "POModelpo",
                table: "AdjustmentFG",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Shipment_detail_POModelpo",
                table: "Shipment_detail",
                column: "POModelpo");

            migrationBuilder.CreateIndex(
                name: "IX_Repack_fromPOModelpo",
                table: "Repack",
                column: "fromPOModelpo");

            migrationBuilder.CreateIndex(
                name: "IX_Repack_toPOModelpo",
                table: "Repack",
                column: "toPOModelpo");

            migrationBuilder.CreateIndex(
                name: "IX_Production_output_POModelpo",
                table: "Production_output",
                column: "POModelpo");

            migrationBuilder.CreateIndex(
                name: "IX_Production_input_POModelpo",
                table: "Production_input",
                column: "POModelpo");

            migrationBuilder.CreateIndex(
                name: "IX_AdjustmentFG_POModelpo",
                table: "AdjustmentFG",
                column: "POModelpo");

            migrationBuilder.AddForeignKey(
                name: "FK_AdjustmentFG_PO_POModelpo",
                table: "AdjustmentFG",
                column: "POModelpo",
                principalTable: "PO",
                principalColumn: "po",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Production_input_PO_POModelpo",
                table: "Production_input",
                column: "POModelpo",
                principalTable: "PO",
                principalColumn: "po",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Production_output_PO_POModelpo",
                table: "Production_output",
                column: "POModelpo",
                principalTable: "PO",
                principalColumn: "po",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Repack_PO_fromPOModelpo",
                table: "Repack",
                column: "fromPOModelpo",
                principalTable: "PO",
                principalColumn: "po",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Repack_PO_toPOModelpo",
                table: "Repack",
                column: "toPOModelpo",
                principalTable: "PO",
                principalColumn: "po",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Shipment_detail_PO_POModelpo",
                table: "Shipment_detail",
                column: "POModelpo",
                principalTable: "PO",
                principalColumn: "po",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
