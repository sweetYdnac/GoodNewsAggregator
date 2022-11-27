using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace by.Reba.DataBase.Migrations
{
    public partial class change_preference_table_naming : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserPreferences_Positivities_PositivityRatingId",
                table: "UserPreferences");

            migrationBuilder.DropForeignKey(
                name: "FK_UserPreferences_Users_UserId",
                table: "UserPreferences");

            migrationBuilder.DropForeignKey(
                name: "FK_UserPreferencesCategories_Categories_CategoriesId",
                table: "UserPreferencesCategories");

            migrationBuilder.DropForeignKey(
                name: "FK_UserPreferencesCategories_UserPreferences_UserPreferencesId",
                table: "UserPreferencesCategories");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserPreferencesCategories",
                table: "UserPreferencesCategories");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserPreferences",
                table: "UserPreferences");

            migrationBuilder.RenameTable(
                name: "UserPreferencesCategories",
                newName: "PreferencesCategories");

            migrationBuilder.RenameTable(
                name: "UserPreferences",
                newName: "Preferences");

            migrationBuilder.RenameColumn(
                name: "UserPreferencesId",
                table: "PreferencesCategories",
                newName: "PreferencesId");

            migrationBuilder.RenameIndex(
                name: "IX_UserPreferencesCategories_UserPreferencesId",
                table: "PreferencesCategories",
                newName: "IX_PreferencesCategories_PreferencesId");

            migrationBuilder.RenameIndex(
                name: "IX_UserPreferences_UserId",
                table: "Preferences",
                newName: "IX_Preferences_UserId");

            //migrationBuilder.RenameIndex(
            //    name: "IX_UserPreferences_PositivityRatingId",
            //    table: "Preferences",
            //    newName: "IX_Preferences_PositivityRatingId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PreferencesCategories",
                table: "PreferencesCategories",
                columns: new[] { "CategoriesId", "PreferencesId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_Preferences",
                table: "Preferences",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Preferences_Positivities_PositivityRatingId",
                table: "Preferences",
                column: "PositivityRatingId",
                principalTable: "Positivities",
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
                name: "FK_PreferencesCategories_Categories_CategoriesId",
                table: "PreferencesCategories",
                column: "CategoriesId",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PreferencesCategories_Preferences_PreferencesId",
                table: "PreferencesCategories",
                column: "PreferencesId",
                principalTable: "Preferences",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Preferences_Positivities_PositivityRatingId",
                table: "Preferences");

            migrationBuilder.DropForeignKey(
                name: "FK_Preferences_Users_UserId",
                table: "Preferences");

            migrationBuilder.DropForeignKey(
                name: "FK_PreferencesCategories_Categories_CategoriesId",
                table: "PreferencesCategories");

            migrationBuilder.DropForeignKey(
                name: "FK_PreferencesCategories_Preferences_PreferencesId",
                table: "PreferencesCategories");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PreferencesCategories",
                table: "PreferencesCategories");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Preferences",
                table: "Preferences");

            migrationBuilder.RenameTable(
                name: "PreferencesCategories",
                newName: "UserPreferencesCategories");

            migrationBuilder.RenameTable(
                name: "Preferences",
                newName: "UserPreferences");

            migrationBuilder.RenameColumn(
                name: "PreferencesId",
                table: "UserPreferencesCategories",
                newName: "UserPreferencesId");

            migrationBuilder.RenameIndex(
                name: "IX_PreferencesCategories_PreferencesId",
                table: "UserPreferencesCategories",
                newName: "IX_UserPreferencesCategories_UserPreferencesId");

            migrationBuilder.RenameIndex(
                name: "IX_Preferences_UserId",
                table: "UserPreferences",
                newName: "IX_UserPreferences_UserId");

            //migrationBuilder.RenameIndex(
            //    name: "IX_Preferences_PositivityRatingId",
            //    table: "UserPreferences",
            //    newName: "IX_UserPreferences_PositivityRatingId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserPreferencesCategories",
                table: "UserPreferencesCategories",
                columns: new[] { "CategoriesId", "UserPreferencesId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserPreferences",
                table: "UserPreferences",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UserPreferences_Positivities_PositivityRatingId",
                table: "UserPreferences",
                column: "PositivityRatingId",
                principalTable: "Positivities",
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
                name: "FK_UserPreferencesCategories_Categories_CategoriesId",
                table: "UserPreferencesCategories",
                column: "CategoriesId",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserPreferencesCategories_UserPreferences_UserPreferencesId",
                table: "UserPreferencesCategories",
                column: "UserPreferencesId",
                principalTable: "UserPreferences",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
