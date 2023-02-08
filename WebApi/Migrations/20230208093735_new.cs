using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace WebApi.Migrations
{
    /// <inheritdoc />
    public partial class @new : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Accounts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    FirstName = table.Column<string>(type: "text", nullable: false),
                    LastName = table.Column<string>(type: "text", nullable: false),
                    Email = table.Column<string>(type: "text", nullable: false),
                    Password = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Accounts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AnimalTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Type = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AnimalTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LocationPoints",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Latitude = table.Column<double>(type: "double precision", nullable: false),
                    Longitude = table.Column<double>(type: "double precision", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LocationPoints", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Animals",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Weight = table.Column<float>(type: "real", nullable: false),
                    Length = table.Column<float>(type: "real", nullable: false),
                    Height = table.Column<float>(type: "real", nullable: false),
                    Gender = table.Column<int>(type: "integer", nullable: false),
                    ChippingDateTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ChipperId = table.Column<int>(type: "integer", nullable: true),
                    ChippingLocationId = table.Column<long>(type: "bigint", nullable: true),
                    DeathDateTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    LifeStatus = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Animals", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Animals_Accounts_ChipperId",
                        column: x => x.ChipperId,
                        principalTable: "Accounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Animals_LocationPoints_ChippingLocationId",
                        column: x => x.ChippingLocationId,
                        principalTable: "LocationPoints",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "AnimalAnimalType",
                columns: table => new
                {
                    AnimalId = table.Column<int>(type: "integer", nullable: false),
                    AnimalTypesId = table.Column<int>(type: "integer", nullable: false)
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
                    table.ForeignKey(
                        name: "FK_AnimalAnimalType_Animals_AnimalId",
                        column: x => x.AnimalId,
                        principalTable: "Animals",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AnimalVisitedLocations",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    DateTimeOfVisitLocationPoint = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    LocationPointId = table.Column<long>(type: "bigint", nullable: true),
                    AnimalId = table.Column<int>(type: "integer", nullable: true)
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
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AnimalAnimalType_AnimalTypesId",
                table: "AnimalAnimalType",
                column: "AnimalTypesId");

            migrationBuilder.CreateIndex(
                name: "IX_Animals_ChipperId",
                table: "Animals",
                column: "ChipperId");

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
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AnimalAnimalType");

            migrationBuilder.DropTable(
                name: "AnimalVisitedLocations");

            migrationBuilder.DropTable(
                name: "AnimalTypes");

            migrationBuilder.DropTable(
                name: "Animals");

            migrationBuilder.DropTable(
                name: "Accounts");

            migrationBuilder.DropTable(
                name: "LocationPoints");
        }
    }
}
