using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Repository.Migrations
{
    /// <inheritdoc />
    public partial class newMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DeliveryTechCompany_Deliveries_DeliveriesId",
                table: "DeliveryTechCompany");

            migrationBuilder.DropForeignKey(
                name: "FK_DeliveryTechCompany_TechCompanies_TechCompaniesId",
                table: "DeliveryTechCompany");

            migrationBuilder.DropIndex(
                name: "IX_Deliveries_OrderId",
                table: "Deliveries");

            migrationBuilder.DropPrimaryKey(
                name: "PK_DeliveryTechCompany",
                table: "DeliveryTechCompany");

            migrationBuilder.DropColumn(
                name: "DeliveryAddress",
                table: "Deliveries");

            migrationBuilder.RenameTable(
                name: "DeliveryTechCompany",
                newName: "DeliveryTechCompanies");

            migrationBuilder.RenameColumn(
                name: "Longitude",
                table: "Deliveries",
                newName: "PickupLongitude");

            migrationBuilder.RenameColumn(
                name: "Latitude",
                table: "Deliveries",
                newName: "PickupLatitude");

            migrationBuilder.RenameColumn(
                name: "IsOnline",
                table: "Deliveries",
                newName: "IsDeleted");

            migrationBuilder.RenameColumn(
                name: "DeliveryLongitude",
                table: "Deliveries",
                newName: "DropoffLongitude");

            migrationBuilder.RenameColumn(
                name: "DeliveryLatitude",
                table: "Deliveries",
                newName: "DropoffLatitude");

            migrationBuilder.RenameColumn(
                name: "TechCompaniesId",
                table: "DeliveryTechCompanies",
                newName: "TechCompanyId");

            migrationBuilder.RenameColumn(
                name: "DeliveriesId",
                table: "DeliveryTechCompanies",
                newName: "DeliveryId");

            migrationBuilder.RenameIndex(
                name: "IX_DeliveryTechCompany_TechCompaniesId",
                table: "DeliveryTechCompanies",
                newName: "IX_DeliveryTechCompanies_TechCompanyId");

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                table: "WishLists",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "WishLists",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                table: "WishListItems",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "WishListItems",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                table: "Warranties",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Warranties",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                table: "TechCompanies",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "TechCompanies",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                table: "SubCategories",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "SubCategories",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                table: "Specifications",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Specifications",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                table: "ServiceUsages",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "ServiceUsages",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                table: "Products",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Products",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                table: "PCAssemblyItems",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "PCAssemblyItems",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                table: "PCAssemblies",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "PCAssemblies",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                table: "Orders",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Orders",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                table: "OrderItems",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "OrderItems",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                table: "OrderHistory",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "OrderHistory",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                table: "Notifications",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Notifications",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                table: "Maintenances",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Maintenances",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<double>(
                name: "CurrentLatitude",
                table: "DeliveryPersons",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "CurrentLongitude",
                table: "DeliveryPersons",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                table: "DeliveryPersons",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "DeliveryPersons",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastOnline",
                table: "DeliveryPersons",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "License",
                table: "DeliveryPersons",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ClusterId",
                table: "DeliveryOffer",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                table: "DeliveryOffer",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "DeliveryOffer",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<double>(
                name: "OfferedPrice",
                table: "DeliveryOffer",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AlterColumn<string>(
                name: "TrackingNumber",
                table: "Deliveries",
                type: "nvarchar(64)",
                maxLength: 64,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "Deliveries",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "PickupAddress",
                table: "Deliveries",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "DeliveryFee",
                table: "Deliveries",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CustomerPhone",
                table: "Deliveries",
                type: "nvarchar(32)",
                maxLength: 32,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CustomerName",
                table: "Deliveries",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                table: "Deliveries",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsFinalLeg",
                table: "Deliveries",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "ParentDeliveryId",
                table: "Deliveries",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "RouteOrder",
                table: "Deliveries",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "SequenceNumber",
                table: "Deliveries",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                table: "Customers",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Customers",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                table: "Categories",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Categories",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                table: "Carts",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Carts",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                table: "CartItems",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "CartItems",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                table: "Admins",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Admins",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddPrimaryKey(
                name: "PK_DeliveryTechCompanies",
                table: "DeliveryTechCompanies",
                columns: new[] { "DeliveryId", "TechCompanyId" });

            migrationBuilder.CreateTable(
                name: "DeliveryAssignmentSettings",
                columns: table => new
                {
                    MaxRetries = table.Column<int>(type: "int", nullable: false),
                    PricePerKm = table.Column<double>(type: "float", nullable: false),
                    MaxOffersPerDriver = table.Column<int>(type: "int", nullable: false),
                    OfferExpiryTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    AssignNearestDriverFirst = table.Column<bool>(type: "bit", nullable: false),
                    AllowMultipleDriversPerCluster = table.Column<bool>(type: "bit", nullable: false),
                    MaxDriverDistanceKm = table.Column<double>(type: "float", nullable: false),
                    CheckInterval = table.Column<TimeSpan>(type: "time", nullable: false),
                    RetryDelay = table.Column<TimeSpan>(type: "time", nullable: false),
                    EnableReassignment = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "DeliveryClusterTracking",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    clusterId = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Location = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    LastUpdated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastRetryTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    Driver = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true, defaultValueSql: "GETUTCDATE()"),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeliveryClusterTracking", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DeliveryCluster",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    DeliveryId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    TechCompanyId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    TechCompanyName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DistanceKm = table.Column<double>(type: "float", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    AssignedDriverId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    AssignedDriverName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AssignmentTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DropoffLatitude = table.Column<double>(type: "float", nullable: true),
                    DropoffLongitude = table.Column<double>(type: "float", nullable: true),
                    SequenceOrder = table.Column<int>(type: "int", nullable: false),
                    EstimatedDistance = table.Column<double>(type: "float", nullable: false),
                    EstimatedPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TrackingId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true, defaultValueSql: "GETUTCDATE()"),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeliveryCluster", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DeliveryCluster_Deliveries_DeliveryId",
                        column: x => x.DeliveryId,
                        principalTable: "Deliveries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DeliveryCluster_DeliveryClusterTracking_TrackingId",
                        column: x => x.TrackingId,
                        principalTable: "DeliveryClusterTracking",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_DeliveryCluster_DeliveryPersons_AssignedDriverId",
                        column: x => x.AssignedDriverId,
                        principalTable: "DeliveryPersons",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_DeliveryCluster_TechCompanies_TechCompanyId",
                        column: x => x.TechCompanyId,
                        principalTable: "TechCompanies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "DeliveryClusterDriverOffer",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    DeliveryClusterId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    DriverId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    OfferTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ResponseTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    OfferedPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true, defaultValueSql: "GETUTCDATE()"),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeliveryClusterDriverOffer", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DeliveryClusterDriverOffer_DeliveryCluster_DeliveryClusterId",
                        column: x => x.DeliveryClusterId,
                        principalTable: "DeliveryCluster",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DeliveryClusterDriverOffer_DeliveryPersons_DriverId",
                        column: x => x.DriverId,
                        principalTable: "DeliveryPersons",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DeliveryOffer_ClusterId",
                table: "DeliveryOffer",
                column: "ClusterId");

            migrationBuilder.CreateIndex(
                name: "IX_Deliveries_OrderId",
                table: "Deliveries",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_Deliveries_ParentDeliveryId",
                table: "Deliveries",
                column: "ParentDeliveryId");

            migrationBuilder.CreateIndex(
                name: "IX_DeliveryCluster_AssignedDriverId",
                table: "DeliveryCluster",
                column: "AssignedDriverId");

            migrationBuilder.CreateIndex(
                name: "IX_DeliveryCluster_DeliveryId",
                table: "DeliveryCluster",
                column: "DeliveryId");

            migrationBuilder.CreateIndex(
                name: "IX_DeliveryCluster_Status",
                table: "DeliveryCluster",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_DeliveryCluster_TechCompanyId",
                table: "DeliveryCluster",
                column: "TechCompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_DeliveryCluster_TrackingId",
                table: "DeliveryCluster",
                column: "TrackingId");

            migrationBuilder.CreateIndex(
                name: "IX_DeliveryClusterDriverOffer_DeliveryClusterId_DriverId",
                table: "DeliveryClusterDriverOffer",
                columns: new[] { "DeliveryClusterId", "DriverId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_DeliveryClusterDriverOffer_DriverId",
                table: "DeliveryClusterDriverOffer",
                column: "DriverId");

            migrationBuilder.AddForeignKey(
                name: "FK_Deliveries_Deliveries_ParentDeliveryId",
                table: "Deliveries",
                column: "ParentDeliveryId",
                principalTable: "Deliveries",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_DeliveryOffer_DeliveryCluster_ClusterId",
                table: "DeliveryOffer",
                column: "ClusterId",
                principalTable: "DeliveryCluster",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_DeliveryTechCompanies_Deliveries_DeliveryId",
                table: "DeliveryTechCompanies",
                column: "DeliveryId",
                principalTable: "Deliveries",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DeliveryTechCompanies_TechCompanies_TechCompanyId",
                table: "DeliveryTechCompanies",
                column: "TechCompanyId",
                principalTable: "TechCompanies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Deliveries_Deliveries_ParentDeliveryId",
                table: "Deliveries");

            migrationBuilder.DropForeignKey(
                name: "FK_DeliveryOffer_DeliveryCluster_ClusterId",
                table: "DeliveryOffer");

            migrationBuilder.DropForeignKey(
                name: "FK_DeliveryTechCompanies_Deliveries_DeliveryId",
                table: "DeliveryTechCompanies");

            migrationBuilder.DropForeignKey(
                name: "FK_DeliveryTechCompanies_TechCompanies_TechCompanyId",
                table: "DeliveryTechCompanies");

            migrationBuilder.DropTable(
                name: "DeliveryAssignmentSettings");

            migrationBuilder.DropTable(
                name: "DeliveryClusterDriverOffer");

            migrationBuilder.DropTable(
                name: "DeliveryCluster");

            migrationBuilder.DropTable(
                name: "DeliveryClusterTracking");

            migrationBuilder.DropIndex(
                name: "IX_DeliveryOffer_ClusterId",
                table: "DeliveryOffer");

            migrationBuilder.DropIndex(
                name: "IX_Deliveries_OrderId",
                table: "Deliveries");

            migrationBuilder.DropIndex(
                name: "IX_Deliveries_ParentDeliveryId",
                table: "Deliveries");

            migrationBuilder.DropPrimaryKey(
                name: "PK_DeliveryTechCompanies",
                table: "DeliveryTechCompanies");

            migrationBuilder.DropColumn(
                name: "DeletedAt",
                table: "WishLists");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "WishLists");

            migrationBuilder.DropColumn(
                name: "DeletedAt",
                table: "WishListItems");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "WishListItems");

            migrationBuilder.DropColumn(
                name: "DeletedAt",
                table: "Warranties");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Warranties");

            migrationBuilder.DropColumn(
                name: "DeletedAt",
                table: "TechCompanies");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "TechCompanies");

            migrationBuilder.DropColumn(
                name: "DeletedAt",
                table: "SubCategories");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "SubCategories");

            migrationBuilder.DropColumn(
                name: "DeletedAt",
                table: "Specifications");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Specifications");

            migrationBuilder.DropColumn(
                name: "DeletedAt",
                table: "ServiceUsages");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "ServiceUsages");

            migrationBuilder.DropColumn(
                name: "DeletedAt",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "DeletedAt",
                table: "PCAssemblyItems");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "PCAssemblyItems");

            migrationBuilder.DropColumn(
                name: "DeletedAt",
                table: "PCAssemblies");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "PCAssemblies");

            migrationBuilder.DropColumn(
                name: "DeletedAt",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "DeletedAt",
                table: "OrderItems");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "OrderItems");

            migrationBuilder.DropColumn(
                name: "DeletedAt",
                table: "OrderHistory");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "OrderHistory");

            migrationBuilder.DropColumn(
                name: "DeletedAt",
                table: "Notifications");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Notifications");

            migrationBuilder.DropColumn(
                name: "DeletedAt",
                table: "Maintenances");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Maintenances");

            migrationBuilder.DropColumn(
                name: "CurrentLatitude",
                table: "DeliveryPersons");

            migrationBuilder.DropColumn(
                name: "CurrentLongitude",
                table: "DeliveryPersons");

            migrationBuilder.DropColumn(
                name: "DeletedAt",
                table: "DeliveryPersons");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "DeliveryPersons");

            migrationBuilder.DropColumn(
                name: "LastOnline",
                table: "DeliveryPersons");

            migrationBuilder.DropColumn(
                name: "License",
                table: "DeliveryPersons");

            migrationBuilder.DropColumn(
                name: "ClusterId",
                table: "DeliveryOffer");

            migrationBuilder.DropColumn(
                name: "DeletedAt",
                table: "DeliveryOffer");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "DeliveryOffer");

            migrationBuilder.DropColumn(
                name: "OfferedPrice",
                table: "DeliveryOffer");

            migrationBuilder.DropColumn(
                name: "DeletedAt",
                table: "Deliveries");

            migrationBuilder.DropColumn(
                name: "IsFinalLeg",
                table: "Deliveries");

            migrationBuilder.DropColumn(
                name: "ParentDeliveryId",
                table: "Deliveries");

            migrationBuilder.DropColumn(
                name: "RouteOrder",
                table: "Deliveries");

            migrationBuilder.DropColumn(
                name: "SequenceNumber",
                table: "Deliveries");

            migrationBuilder.DropColumn(
                name: "DeletedAt",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "DeletedAt",
                table: "Categories");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Categories");

            migrationBuilder.DropColumn(
                name: "DeletedAt",
                table: "Carts");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Carts");

            migrationBuilder.DropColumn(
                name: "DeletedAt",
                table: "CartItems");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "CartItems");

            migrationBuilder.DropColumn(
                name: "DeletedAt",
                table: "Admins");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Admins");

            migrationBuilder.RenameTable(
                name: "DeliveryTechCompanies",
                newName: "DeliveryTechCompany");

            migrationBuilder.RenameColumn(
                name: "PickupLongitude",
                table: "Deliveries",
                newName: "Longitude");

            migrationBuilder.RenameColumn(
                name: "PickupLatitude",
                table: "Deliveries",
                newName: "Latitude");

            migrationBuilder.RenameColumn(
                name: "IsDeleted",
                table: "Deliveries",
                newName: "IsOnline");

            migrationBuilder.RenameColumn(
                name: "DropoffLongitude",
                table: "Deliveries",
                newName: "DeliveryLongitude");

            migrationBuilder.RenameColumn(
                name: "DropoffLatitude",
                table: "Deliveries",
                newName: "DeliveryLatitude");

            migrationBuilder.RenameColumn(
                name: "TechCompanyId",
                table: "DeliveryTechCompany",
                newName: "TechCompaniesId");

            migrationBuilder.RenameColumn(
                name: "DeliveryId",
                table: "DeliveryTechCompany",
                newName: "DeliveriesId");

            migrationBuilder.RenameIndex(
                name: "IX_DeliveryTechCompanies_TechCompanyId",
                table: "DeliveryTechCompany",
                newName: "IX_DeliveryTechCompany_TechCompaniesId");

            migrationBuilder.AlterColumn<string>(
                name: "TrackingNumber",
                table: "Deliveries",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(64)",
                oldMaxLength: 64,
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "Status",
                table: "Deliveries",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "PickupAddress",
                table: "Deliveries",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldMaxLength: 500,
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "DeliveryFee",
                table: "Deliveries",
                type: "decimal(18,2)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)",
                oldDefaultValue: 0m);

            migrationBuilder.AlterColumn<string>(
                name: "CustomerPhone",
                table: "Deliveries",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(32)",
                oldMaxLength: 32,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CustomerName",
                table: "Deliveries",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldMaxLength: 200,
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DeliveryAddress",
                table: "Deliveries",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_DeliveryTechCompany",
                table: "DeliveryTechCompany",
                columns: new[] { "DeliveriesId", "TechCompaniesId" });

            migrationBuilder.CreateIndex(
                name: "IX_Deliveries_OrderId",
                table: "Deliveries",
                column: "OrderId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_DeliveryTechCompany_Deliveries_DeliveriesId",
                table: "DeliveryTechCompany",
                column: "DeliveriesId",
                principalTable: "Deliveries",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DeliveryTechCompany_TechCompanies_TechCompaniesId",
                table: "DeliveryTechCompany",
                column: "TechCompaniesId",
                principalTable: "TechCompanies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
