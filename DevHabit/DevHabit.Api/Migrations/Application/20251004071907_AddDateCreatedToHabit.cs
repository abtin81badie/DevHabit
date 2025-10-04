using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DevHabit.Api.Migrations.Application
{
    /// <inheritdoc />
    public partial class AddDateCreatedToHabit : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "pk_habit",
                schema: "dev_habit",
                table: "habit");

            migrationBuilder.RenameTable(
                name: "habit",
                schema: "dev_habit",
                newName: "habits",
                newSchema: "dev_habit");

            migrationBuilder.AddPrimaryKey(
                name: "pk_habits",
                schema: "dev_habit",
                table: "habits",
                column: "id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "pk_habits",
                schema: "dev_habit",
                table: "habits");

            migrationBuilder.RenameTable(
                name: "habits",
                schema: "dev_habit",
                newName: "habit",
                newSchema: "dev_habit");

            migrationBuilder.AddPrimaryKey(
                name: "pk_habit",
                schema: "dev_habit",
                table: "habit",
                column: "id");
        }
    }
}
