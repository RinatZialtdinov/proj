using Microsoft.EntityFrameworkCore.Migrations;

namespace WebMaterial.Migrations
{
    public partial class addextension : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Extensio",
                table: "Materials",
                newName: "Extension");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Extension",
                table: "Materials",
                newName: "Extensio");
        }
    }
}
