using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Repository.Migrations
{
    /// <inheritdoc />
    public partial class newMig : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "DeliveryPersons",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<double>(
                name: "DeliveryLatitude",
                table: "Deliveries",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "DeliveryLongitude",
                table: "Deliveries",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsOnline",
                table: "Deliveries",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<double>(
                name: "Latitude",
                table: "Deliveries",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "Longitude",
                table: "Deliveries",
                type: "float",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Name",
                table: "DeliveryPersons");

            migrationBuilder.DropColumn(
                name: "DeliveryLatitude",
                table: "Deliveries");

            migrationBuilder.DropColumn(
                name: "DeliveryLongitude",
                table: "Deliveries");

            migrationBuilder.DropColumn(
                name: "IsOnline",
                table: "Deliveries");

            migrationBuilder.DropColumn(
                name: "Latitude",
                table: "Deliveries");

            migrationBuilder.DropColumn(
                name: "Longitude",
                table: "Deliveries");
        }
    }
}
