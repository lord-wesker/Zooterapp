using Microsoft.EntityFrameworkCore.Migrations;

namespace Zooterapp.Web.Migrations
{
    public partial class PetRacePascalCase : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "race",
                table: "Pets",
                newName: "Race");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Race",
                table: "Pets",
                newName: "race");
        }
    }
}
