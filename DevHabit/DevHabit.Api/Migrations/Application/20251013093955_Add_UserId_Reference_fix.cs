using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DevHabit.Api.Migrations.Application
{
    /// <inheritdoc />
    public partial class Add_UserId_Reference_fix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_tags_users_id",
                schema: "dev_habit",
                table: "tags");

            migrationBuilder.AddForeignKey(
                name: "fk_tags_users_user_id",
                schema: "dev_habit",
                table: "tags",
                column: "user_id",
                principalSchema: "dev_habit",
                principalTable: "users",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_tags_users_user_id",
                schema: "dev_habit",
                table: "tags");

            migrationBuilder.AddForeignKey(
                name: "fk_tags_users_id",
                schema: "dev_habit",
                table: "tags",
                column: "id",
                principalSchema: "dev_habit",
                principalTable: "users",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
