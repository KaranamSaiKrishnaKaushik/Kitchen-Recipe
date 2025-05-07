using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Migrations
{
    /// <inheritdoc />
    public partial class AlterUserDetailsTableAddMoreColumns : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "UserName",
                table: "UserDetails",
                newName: "ZipCode");

            migrationBuilder.RenameColumn(
                name: "Address",
                table: "UserDetails",
                newName: "YoutubeUserName");

            migrationBuilder.AddColumn<string>(
                name: "Biography",
                table: "UserDetails",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "City",
                table: "UserDetails",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Country",
                table: "UserDetails",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "FacobookUserName",
                table: "UserDetails",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Headline",
                table: "UserDetails",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "HouseNumber",
                table: "UserDetails",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "InstagramUserName",
                table: "UserDetails",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Language",
                table: "UserDetails",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "LinkedInPublicProfileUrl",
                table: "UserDetails",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "State",
                table: "UserDetails",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "SteetAddress",
                table: "UserDetails",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "TwitterUserName",
                table: "UserDetails",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "UserFirstName",
                table: "UserDetails",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "UserLastName",
                table: "UserDetails",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Website",
                table: "UserDetails",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Biography",
                table: "UserDetails");

            migrationBuilder.DropColumn(
                name: "City",
                table: "UserDetails");

            migrationBuilder.DropColumn(
                name: "Country",
                table: "UserDetails");

            migrationBuilder.DropColumn(
                name: "FacobookUserName",
                table: "UserDetails");

            migrationBuilder.DropColumn(
                name: "Headline",
                table: "UserDetails");

            migrationBuilder.DropColumn(
                name: "HouseNumber",
                table: "UserDetails");

            migrationBuilder.DropColumn(
                name: "InstagramUserName",
                table: "UserDetails");

            migrationBuilder.DropColumn(
                name: "Language",
                table: "UserDetails");

            migrationBuilder.DropColumn(
                name: "LinkedInPublicProfileUrl",
                table: "UserDetails");

            migrationBuilder.DropColumn(
                name: "State",
                table: "UserDetails");

            migrationBuilder.DropColumn(
                name: "SteetAddress",
                table: "UserDetails");

            migrationBuilder.DropColumn(
                name: "TwitterUserName",
                table: "UserDetails");

            migrationBuilder.DropColumn(
                name: "UserFirstName",
                table: "UserDetails");

            migrationBuilder.DropColumn(
                name: "UserLastName",
                table: "UserDetails");

            migrationBuilder.DropColumn(
                name: "Website",
                table: "UserDetails");

            migrationBuilder.RenameColumn(
                name: "ZipCode",
                table: "UserDetails",
                newName: "UserName");

            migrationBuilder.RenameColumn(
                name: "YoutubeUserName",
                table: "UserDetails",
                newName: "Address");
        }
    }
}
