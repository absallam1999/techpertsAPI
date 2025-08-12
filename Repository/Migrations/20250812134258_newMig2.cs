using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Repository.Migrations
{
    /// <inheritdoc />
    public partial class newMig2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Image1Url",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "Image2Url",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "Image3Url",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "Image4Url",
                table: "Products");

            migrationBuilder.AddColumn<string>(
                name: "ImagesURLS",
                table: "Products",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImagesURLS",
                table: "Products");

            migrationBuilder.AddColumn<string>(
                name: "Image1Url",
                table: "Products",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Image2Url",
                table: "Products",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Image3Url",
                table: "Products",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Image4Url",
                table: "Products",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
