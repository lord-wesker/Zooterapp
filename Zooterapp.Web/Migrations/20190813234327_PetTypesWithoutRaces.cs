using Microsoft.EntityFrameworkCore.Migrations;

namespace Zooterapp.Web.Migrations
{
    public partial class PetTypesWithoutRaces : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "race",
                table: "PetTypes");

            migrationBuilder.AddColumn<string>(
                name: "race",
                table: "Pets",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "race",
                table: "Pets");

            migrationBuilder.AddColumn<string>(
                name: "race",
                table: "PetTypes",
                nullable: true);
        }
    }
}
