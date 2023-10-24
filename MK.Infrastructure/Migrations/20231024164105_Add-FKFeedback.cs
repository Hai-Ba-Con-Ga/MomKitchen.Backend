using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MK.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddFKFeedback : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_tray_meal_meal_id",
                table: "tray");

            migrationBuilder.DropIndex(
                name: "ix_tray_meal_id",
                table: "tray");

            migrationBuilder.DropColumn(
                name: "meal_id",
                table: "tray");

            migrationBuilder.AlterColumn<string>(
                name: "img_url",
                table: "feedback",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddColumn<Guid>(
                name: "kitchen_id",
                table: "feedback",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "ix_feedback_kitchen_id",
                table: "feedback",
                column: "kitchen_id");

            migrationBuilder.AddForeignKey(
                name: "fk_feedback_kitchen_kitchen_id",
                table: "feedback",
                column: "kitchen_id",
                principalTable: "kitchen",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_feedback_kitchen_kitchen_id",
                table: "feedback");

            migrationBuilder.DropIndex(
                name: "ix_feedback_kitchen_id",
                table: "feedback");

            migrationBuilder.DropColumn(
                name: "kitchen_id",
                table: "feedback");

            migrationBuilder.AddColumn<Guid>(
                name: "meal_id",
                table: "tray",
                type: "uuid",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "img_url",
                table: "feedback",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "ix_tray_meal_id",
                table: "tray",
                column: "meal_id");

            migrationBuilder.AddForeignKey(
                name: "fk_tray_meal_meal_id",
                table: "tray",
                column: "meal_id",
                principalTable: "meal",
                principalColumn: "id");
        }
    }
}
