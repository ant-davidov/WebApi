using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApi.Migrations
{
    /// <inheritdoc />
    public partial class first : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Animals_AnimalVisitedLocations_ChippingLocationId",
                table: "Animals");

            migrationBuilder.AlterColumn<DateTime>(
                name: "DeathDateTime",
                table: "Animals",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "TEXT");

            migrationBuilder.CreateIndex(
                name: "IX_Animals_ChipperId",
                table: "Animals",
                column: "ChipperId");

            migrationBuilder.AddForeignKey(
                name: "FK_Animals_Accounts_ChipperId",
                table: "Animals",
                column: "ChipperId",
                principalTable: "Accounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Animals_LocationPoints_ChippingLocationId",
                table: "Animals",
                column: "ChippingLocationId",
                principalTable: "LocationPoints",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Animals_Accounts_ChipperId",
                table: "Animals");

            migrationBuilder.DropForeignKey(
                name: "FK_Animals_LocationPoints_ChippingLocationId",
                table: "Animals");

            migrationBuilder.DropIndex(
                name: "IX_Animals_ChipperId",
                table: "Animals");

            migrationBuilder.AlterColumn<DateTime>(
                name: "DeathDateTime",
                table: "Animals",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Animals_AnimalVisitedLocations_ChippingLocationId",
                table: "Animals",
                column: "ChippingLocationId",
                principalTable: "AnimalVisitedLocations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
