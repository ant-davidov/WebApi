using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApi.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Accounts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    FirstName = table.Column<string>(type: "TEXT", nullable: false),
                    LastName = table.Column<string>(type: "TEXT", nullable: false),
                    Email = table.Column<string>(type: "TEXT", nullable: false),
                    Password = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Accounts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AnimalTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Type = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AnimalTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LocationPoints",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Latitude = table.Column<double>(type: "REAL", nullable: false),
                    Longitude = table.Column<double>(type: "REAL", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LocationPoints", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AnimalAnimalType",
                columns: table => new
                {
                    AnimalId = table.Column<int>(type: "INTEGER", nullable: false),
                    AnimalTypesId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AnimalAnimalType", x => new { x.AnimalId, x.AnimalTypesId });
                    table.ForeignKey(
                        name: "FK_AnimalAnimalType_AnimalTypes_AnimalTypesId",
                        column: x => x.AnimalTypesId,
                        principalTable: "AnimalTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Animals",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Weight = table.Column<float>(type: "REAL", nullable: false),
                    Lenght = table.Column<float>(type: "REAL", nullable: false),
                    Height = table.Column<float>(type: "REAL", nullable: false),
                    Gender = table.Column<int>(type: "INTEGER", nullable: false),
                    ChippingDateTime = table.Column<DateTime>(type: "TEXT", nullable: false),
                    ChipperId = table.Column<int>(type: "INTEGER", nullable: false),
                    ChippingLocationId = table.Column<int>(type: "INTEGER", nullable: false),
                    DeathDateTime = table.Column<DateTime>(type: "TEXT", nullable: false),
                    LifeStatus = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Animals", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AnimalVisitedLocations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    DateTimeOfVisitLocationPoint = table.Column<DateTime>(type: "TEXT", nullable: false),
                    LocationPointId = table.Column<int>(type: "INTEGER", nullable: false),
                    AnimalId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AnimalVisitedLocations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AnimalVisitedLocations_Animals_AnimalId",
                        column: x => x.AnimalId,
                        principalTable: "Animals",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AnimalVisitedLocations_LocationPoints_LocationPointId",
                        column: x => x.LocationPointId,
                        principalTable: "LocationPoints",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AnimalAnimalType_AnimalTypesId",
                table: "AnimalAnimalType",
                column: "AnimalTypesId");

            migrationBuilder.CreateIndex(
                name: "IX_Animals_ChippingLocationId",
                table: "Animals",
                column: "ChippingLocationId");

            migrationBuilder.CreateIndex(
                name: "IX_AnimalVisitedLocations_AnimalId",
                table: "AnimalVisitedLocations",
                column: "AnimalId");

            migrationBuilder.CreateIndex(
                name: "IX_AnimalVisitedLocations_LocationPointId",
                table: "AnimalVisitedLocations",
                column: "LocationPointId");

            migrationBuilder.AddForeignKey(
                name: "FK_AnimalAnimalType_Animals_AnimalId",
                table: "AnimalAnimalType",
                column: "AnimalId",
                principalTable: "Animals",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Animals_AnimalVisitedLocations_ChippingLocationId",
                table: "Animals",
                column: "ChippingLocationId",
                principalTable: "AnimalVisitedLocations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AnimalVisitedLocations_Animals_AnimalId",
                table: "AnimalVisitedLocations");

            migrationBuilder.DropTable(
                name: "Accounts");

            migrationBuilder.DropTable(
                name: "AnimalAnimalType");

            migrationBuilder.DropTable(
                name: "AnimalTypes");

            migrationBuilder.DropTable(
                name: "Animals");

            migrationBuilder.DropTable(
                name: "AnimalVisitedLocations");

            migrationBuilder.DropTable(
                name: "LocationPoints");
        }
    }
}
