using Microsoft.EntityFrameworkCore.Migrations;

namespace Zooterapp.Web.Migrations
{
    public partial class DatabaseRebuild : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Pets_PetTypes_TypeId",
                table: "Pets");

            migrationBuilder.RenameColumn(
                name: "TypeId",
                table: "Pets",
                newName: "PetTypeId");

            migrationBuilder.RenameIndex(
                name: "IX_Pets_TypeId",
                table: "Pets",
                newName: "IX_Pets_PetTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Pets_PetTypes_PetTypeId",
                table: "Pets",
                column: "PetTypeId",
                principalTable: "PetTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Pets_PetTypes_PetTypeId",
                table: "Pets");

            migrationBuilder.RenameColumn(
                name: "PetTypeId",
                table: "Pets",
                newName: "TypeId");

            migrationBuilder.RenameIndex(
                name: "IX_Pets_PetTypeId",
                table: "Pets",
                newName: "IX_Pets_TypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Pets_PetTypes_TypeId",
                table: "Pets",
                column: "TypeId",
                principalTable: "PetTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
