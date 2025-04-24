using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Migrations
{
    /// <inheritdoc />
    public partial class RenameNameToBaseName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RecipeIngredients_IngredientBase_NameId",
                table: "RecipeIngredients");

            migrationBuilder.RenameColumn(
                name: "NameId",
                table: "RecipeIngredients",
                newName: "BaseNameId");

            migrationBuilder.RenameIndex(
                name: "IX_RecipeIngredients_NameId",
                table: "RecipeIngredients",
                newName: "IX_RecipeIngredients_BaseNameId");

            migrationBuilder.AddForeignKey(
                name: "FK_RecipeIngredients_IngredientBase_BaseNameId",
                table: "RecipeIngredients",
                column: "BaseNameId",
                principalTable: "IngredientBase",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RecipeIngredients_IngredientBase_BaseNameId",
                table: "RecipeIngredients");

            migrationBuilder.RenameColumn(
                name: "BaseNameId",
                table: "RecipeIngredients",
                newName: "NameId");

            migrationBuilder.RenameIndex(
                name: "IX_RecipeIngredients_BaseNameId",
                table: "RecipeIngredients",
                newName: "IX_RecipeIngredients_NameId");

            migrationBuilder.AddForeignKey(
                name: "FK_RecipeIngredients_IngredientBase_NameId",
                table: "RecipeIngredients",
                column: "NameId",
                principalTable: "IngredientBase",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
