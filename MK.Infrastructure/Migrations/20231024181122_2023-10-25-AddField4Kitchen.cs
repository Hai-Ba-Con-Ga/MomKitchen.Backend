using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MK.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class _20231025AddField4Kitchen : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "img_url",
                table: "kitchen",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "img_url",
                table: "kitchen");
        }
    }
}
