using Microsoft.EntityFrameworkCore.Migrations;

namespace WebMaterial.Migrations
{
    public partial class add_extensio : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Extensio",
                table: "Materials",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Extensio",
                table: "Materials");
        }
    }
}
