using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Migrations
{
    /// <inheritdoc />
    public partial class AlterUserDetailsTablespellChecks2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "SteetAddress",
                table: "UserDetails",
                newName: "TiktokUserName");

            migrationBuilder.RenameColumn(
                name: "FacobookUserName",
                table: "UserDetails",
                newName: "StreetAddress");

            migrationBuilder.AddColumn<string>(
                name: "FacebookUserName",
                table: "UserDetails",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FacebookUserName",
                table: "UserDetails");

            migrationBuilder.RenameColumn(
                name: "TiktokUserName",
                table: "UserDetails",
                newName: "SteetAddress");

            migrationBuilder.RenameColumn(
                name: "StreetAddress",
                table: "UserDetails",
                newName: "FacobookUserName");
        }
    }
}
