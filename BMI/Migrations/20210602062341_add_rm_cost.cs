using Microsoft.EntityFrameworkCore.Migrations;

namespace BMI.Migrations
{
    public partial class add_rm_cost : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Material",
                table: "PO");

            migrationBuilder.CreateTable(
                name: "Rm_Cost",
                columns: table => new
                {
                    Id_Material = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Material = table.Column<string>(nullable: true),
                    PO = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rm_Cost", x => x.Id_Material);
                    table.ForeignKey(
                        name: "FK_Rm_Cost_PO_PO",
                        column: x => x.PO,
                        principalTable: "PO",
                        principalColumn: "po",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Rm_Cost_PO",
                table: "Rm_Cost",
                column: "PO");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Rm_Cost");

            migrationBuilder.AddColumn<string>(
                name: "Material",
                table: "PO",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
