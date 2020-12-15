using Microsoft.EntityFrameworkCore.Migrations;

namespace BMI.Migrations
{
    public partial class add_pending_table : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Pending",
                columns: table => new
                {
                    id_pending = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    raw_source = table.Column<string>(nullable: true),
                    qty = table.Column<float>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pending", x => x.id_pending);
                    table.ForeignKey(
                        name: "FK_Pending_Rm_raw_source",
                        column: x => x.raw_source,
                        principalTable: "Rm",
                        principalColumn: "raw_source",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Pending_raw_source",
                table: "Pending",
                column: "raw_source");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Pending");
        }
    }
}
