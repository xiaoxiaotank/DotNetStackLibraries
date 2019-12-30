using Microsoft.EntityFrameworkCore.Migrations;

namespace SQL.TransactionIsolationLevel.Migrations
{
    public partial class AddTableAPropertyHasUpdatedIndex : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_As_HasUpdated",
                table: "As",
                column: "HasUpdated");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_As_HasUpdated",
                table: "As");
        }
    }
}
