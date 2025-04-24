using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Migrations
{
    /// <inheritdoc />
    public partial class RenameBaseNameIdToBaseName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Ingredient_IngredientBase_BaseNameIdId",
                table: "Ingredient");

            migrationBuilder.RenameColumn(
                name: "BaseNameIdId",
                table: "Ingredient",
                newName: "BaseNameId");

            migrationBuilder.RenameIndex(
                name: "IX_Ingredient_BaseNameIdId",
                table: "Ingredient",
                newName: "IX_Ingredient_BaseNameId");

            migrationBuilder.AddForeignKey(
                name: "FK_Ingredient_IngredientBase_BaseNameId",
                table: "Ingredient",
                column: "BaseNameId",
                principalTable: "IngredientBase",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Ingredient_IngredientBase_BaseNameId",
                table: "Ingredient");

            migrationBuilder.RenameColumn(
                name: "BaseNameId",
                table: "Ingredient",
                newName: "BaseNameIdId");

            migrationBuilder.RenameIndex(
                name: "IX_Ingredient_BaseNameId",
                table: "Ingredient",
                newName: "IX_Ingredient_BaseNameIdId");

            migrationBuilder.AddForeignKey(
                name: "FK_Ingredient_IngredientBase_BaseNameIdId",
                table: "Ingredient",
                column: "BaseNameIdId",
                principalTable: "IngredientBase",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
