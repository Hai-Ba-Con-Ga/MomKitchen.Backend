using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MK.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class addrelation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "kitchen_id",
                table: "meal",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "ix_meal_kitchen_id",
                table: "meal",
                column: "kitchen_id");

            migrationBuilder.AddForeignKey(
                name: "fk_meal_kitchen_kitchen_id",
                table: "meal",
                column: "kitchen_id",
                principalTable: "kitchen",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_meal_kitchen_kitchen_id",
                table: "meal");

            migrationBuilder.DropIndex(
                name: "ix_meal_kitchen_id",
                table: "meal");

            migrationBuilder.DropColumn(
                name: "kitchen_id",
                table: "meal");
        }
    }
}
