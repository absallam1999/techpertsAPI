using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Repository.Migrations
{
    /// <inheritdoc />
    public partial class RatingsTechCompany : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Website",
                table: "TechCompanies");

            migrationBuilder.AddColumn<decimal>(
                name: "Rating",
                table: "TechCompanies",
                type: "decimal(18,2)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Rating",
                table: "TechCompanies");

            migrationBuilder.AddColumn<string>(
                name: "Website",
                table: "TechCompanies",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
