using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace by.Reba.DataBase.Migrations
{
    public partial class update_tables_naming : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Preferences_PositivityRatings_PositivityRatingId",
                table: "Preferences");

            migrationBuilder.DropForeignKey(
                name: "FK_Preferences_Users_UserId",
                table: "Preferences");

            migrationBuilder.DropForeignKey(
                name: "FK_UsersPreferences_Preferences_UserPreferencesId",
                table: "UsersPreferences");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Preferences",
                table: "Preferences");

            migrationBuilder.RenameTable(
                name: "Preferences",
                newName: "UserPreferences");

            migrationBuilder.RenameIndex(
                name: "IX_Preferences_UserId",
                table: "UserPreferences",
                newName: "IX_UserPreferences_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Preferences_PositivityRatingId",
                table: "UserPreferences",
                newName: "IX_UserPreferences_PositivityRatingId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserPreferences",
                table: "UserPreferences",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UserPreferences_PositivityRatings_PositivityRatingId",
                table: "UserPreferences",
                column: "PositivityRatingId",
                principalTable: "PositivityRatings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserPreferences_Users_UserId",
                table: "UserPreferences",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UsersPreferences_UserPreferences_UserPreferencesId",
                table: "UsersPreferences",
                column: "UserPreferencesId",
                principalTable: "UserPreferences",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserPreferences_PositivityRatings_PositivityRatingId",
                table: "UserPreferences");

            migrationBuilder.DropForeignKey(
                name: "FK_UserPreferences_Users_UserId",
                table: "UserPreferences");

            migrationBuilder.DropForeignKey(
                name: "FK_UsersPreferences_UserPreferences_UserPreferencesId",
                table: "UsersPreferences");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserPreferences",
                table: "UserPreferences");

            migrationBuilder.RenameTable(
                name: "UserPreferences",
                newName: "Preferences");

            migrationBuilder.RenameIndex(
                name: "IX_UserPreferences_UserId",
                table: "Preferences",
                newName: "IX_Preferences_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_UserPreferences_PositivityRatingId",
                table: "Preferences",
                newName: "IX_Preferences_PositivityRatingId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Preferences",
                table: "Preferences",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Preferences_PositivityRatings_PositivityRatingId",
                table: "Preferences",
                column: "PositivityRatingId",
                principalTable: "PositivityRatings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Preferences_Users_UserId",
                table: "Preferences",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UsersPreferences_Preferences_UserPreferencesId",
                table: "UsersPreferences",
                column: "UserPreferencesId",
                principalTable: "Preferences",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
