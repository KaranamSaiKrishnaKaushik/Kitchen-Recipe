using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Migrations
{
    /// <inheritdoc />
    public partial class DeleteProductReweChangeTableName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_ReweProducts",
                table: "ReweProducts");

            migrationBuilder.RenameTable(
                name: "ReweProducts",
                newName: "AllStoresProducts");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AllStoresProducts",
                table: "AllStoresProducts",
                column: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_AllStoresProducts",
                table: "AllStoresProducts");

            migrationBuilder.RenameTable(
                name: "AllStoresProducts",
                newName: "ReweProducts");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ReweProducts",
                table: "ReweProducts",
                column: "Id");
        }
    }
}
