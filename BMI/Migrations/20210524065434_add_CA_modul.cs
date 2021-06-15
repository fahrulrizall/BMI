using Microsoft.EntityFrameworkCore.Migrations;

namespace BMI.Migrations
{
    public partial class add_CA_modul : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<float>(
                name: "PF3710",
                table: "Master_data",
                nullable: true);

            migrationBuilder.AddColumn<float>(
                name: "PF3770",
                table: "Master_data",
                nullable: true);

            migrationBuilder.AddColumn<float>(
                name: "standard_price",
                table: "Master_data",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "CostAnalyst",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PO = table.Column<string>(nullable: true),
                    SAP_Code = table.Column<string>(nullable: true),
                    Target_Lbs = table.Column<float>(nullable: false),
                    Material = table.Column<string>(nullable: true),
                    Price = table.Column<float>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CostAnalyst", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CostAnalyst_PO_PO",
                        column: x => x.PO,
                        principalTable: "PO",
                        principalColumn: "po",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CostAnalyst_PO",
                table: "CostAnalyst",
                column: "PO");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CostAnalyst");

            migrationBuilder.DropColumn(
                name: "PF3710",
                table: "Master_data");

            migrationBuilder.DropColumn(
                name: "PF3770",
                table: "Master_data");

            migrationBuilder.DropColumn(
                name: "standard_price",
                table: "Master_data");
        }
    }
}
