using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Lawliet.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigration6 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CompletedTask",
                table: "StudentTasks");

            migrationBuilder.AddColumn<string>(
                name: "CompletedTask",
                table: "Users",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CompletedTask",
                table: "Users");

            migrationBuilder.AddColumn<string>(
                name: "CompletedTask",
                table: "StudentTasks",
                type: "text",
                nullable: false,
                defaultValue: "");
        }
    }
}
