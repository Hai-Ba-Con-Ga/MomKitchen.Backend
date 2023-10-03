using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MK.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class _20231003UpdateArea : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_area_location_east_id",
                table: "area");

            migrationBuilder.DropForeignKey(
                name: "fk_area_location_north_id",
                table: "area");

            migrationBuilder.DropForeignKey(
                name: "fk_area_location_south_id",
                table: "area");

            migrationBuilder.DropForeignKey(
                name: "fk_area_location_west_id",
                table: "area");

            migrationBuilder.DropIndex(
                name: "ix_area_east_id",
                table: "area");

            migrationBuilder.DropIndex(
                name: "ix_area_north_id",
                table: "area");

            migrationBuilder.DropIndex(
                name: "ix_area_south_id",
                table: "area");

            migrationBuilder.DropIndex(
                name: "ix_area_west_id",
                table: "area");

            migrationBuilder.DropColumn(
                name: "east_id",
                table: "area");

            migrationBuilder.DropColumn(
                name: "north_id",
                table: "area");

            migrationBuilder.DropColumn(
                name: "south_id",
                table: "area");

            migrationBuilder.DropColumn(
                name: "west_id",
                table: "area");

            migrationBuilder.AddColumn<Guid[]>(
                name: "boundaries",
                table: "area",
                type: "uuid[]",
                nullable: false,
                defaultValue: new Guid[0]);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "boundaries",
                table: "area");

            migrationBuilder.AddColumn<Guid>(
                name: "east_id",
                table: "area",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "north_id",
                table: "area",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "south_id",
                table: "area",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "west_id",
                table: "area",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "ix_area_east_id",
                table: "area",
                column: "east_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_area_north_id",
                table: "area",
                column: "north_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_area_south_id",
                table: "area",
                column: "south_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_area_west_id",
                table: "area",
                column: "west_id",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "fk_area_location_east_id",
                table: "area",
                column: "east_id",
                principalTable: "location",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_area_location_north_id",
                table: "area",
                column: "north_id",
                principalTable: "location",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_area_location_south_id",
                table: "area",
                column: "south_id",
                principalTable: "location",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_area_location_west_id",
                table: "area",
                column: "west_id",
                principalTable: "location",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
