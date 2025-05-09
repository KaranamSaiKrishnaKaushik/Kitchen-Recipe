using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Migrations
{
    /// <inheritdoc />
    public partial class AlterTableRecipeChangeUniqueKey : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Recipe_Name",
                table: "Recipe");

            migrationBuilder.AlterColumn<string>(
                name: "AuthenticationUid",
                table: "Recipe",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateIndex(
                name: "IX_Recipe_Name_AuthenticationUid",
                table: "Recipe",
                columns: new[] { "Name", "AuthenticationUid" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Recipe_Name_AuthenticationUid",
                table: "Recipe");

            migrationBuilder.AlterColumn<string>(
                name: "AuthenticationUid",
                table: "Recipe",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.CreateIndex(
                name: "IX_Recipe_Name",
                table: "Recipe",
                column: "Name",
                unique: true);
        }
    }
}
