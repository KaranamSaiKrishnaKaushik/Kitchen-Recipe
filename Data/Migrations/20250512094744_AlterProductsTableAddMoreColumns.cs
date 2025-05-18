using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Migrations
{
    /// <inheritdoc />
    public partial class AlterProductsTableAddMoreColumns : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Category",
                table: "WalmartProducts",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Grammage",
                table: "WalmartProducts",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "IsOnSale",
                table: "WalmartProducts",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "ProductId",
                table: "WalmartProducts",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "sourceName",
                table: "WalmartProducts",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Category",
                table: "ReweProducts",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Grammage",
                table: "ReweProducts",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "IsOnSale",
                table: "ReweProducts",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "ProductId",
                table: "ReweProducts",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "sourceName",
                table: "ReweProducts",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Category",
                table: "AmazonProducts",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Grammage",
                table: "AmazonProducts",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "IsOnSale",
                table: "AmazonProducts",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "ProductId",
                table: "AmazonProducts",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "sourceName",
                table: "AmazonProducts",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Category",
                table: "WalmartProducts");

            migrationBuilder.DropColumn(
                name: "Grammage",
                table: "WalmartProducts");

            migrationBuilder.DropColumn(
                name: "IsOnSale",
                table: "WalmartProducts");

            migrationBuilder.DropColumn(
                name: "ProductId",
                table: "WalmartProducts");

            migrationBuilder.DropColumn(
                name: "sourceName",
                table: "WalmartProducts");

            migrationBuilder.DropColumn(
                name: "Category",
                table: "ReweProducts");

            migrationBuilder.DropColumn(
                name: "Grammage",
                table: "ReweProducts");

            migrationBuilder.DropColumn(
                name: "IsOnSale",
                table: "ReweProducts");

            migrationBuilder.DropColumn(
                name: "ProductId",
                table: "ReweProducts");

            migrationBuilder.DropColumn(
                name: "sourceName",
                table: "ReweProducts");

            migrationBuilder.DropColumn(
                name: "Category",
                table: "AmazonProducts");

            migrationBuilder.DropColumn(
                name: "Grammage",
                table: "AmazonProducts");

            migrationBuilder.DropColumn(
                name: "IsOnSale",
                table: "AmazonProducts");

            migrationBuilder.DropColumn(
                name: "ProductId",
                table: "AmazonProducts");

            migrationBuilder.DropColumn(
                name: "sourceName",
                table: "AmazonProducts");
        }
    }
}
