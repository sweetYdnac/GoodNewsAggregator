using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace by.Reba.DataBase.Migrations
{
    public partial class fix_UserPreferencesCategories_join_table_naming : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UsersPreferences_Categories_CategoriesId",
                table: "UsersPreferences");

            migrationBuilder.DropForeignKey(
                name: "FK_UsersPreferences_UserPreferences_UserPreferencesId",
                table: "UsersPreferences");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UsersPreferences",
                table: "UsersPreferences");

            migrationBuilder.RenameTable(
                name: "UsersPreferences",
                newName: "UserPreferencesCategories");

            migrationBuilder.RenameIndex(
                name: "IX_UsersPreferences_UserPreferencesId",
                table: "UserPreferencesCategories",
                newName: "IX_UserPreferencesCategories_UserPreferencesId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserPreferencesCategories",
                table: "UserPreferencesCategories",
                columns: new[] { "CategoriesId", "UserPreferencesId" });

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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserPreferencesCategories_Categories_CategoriesId",
                table: "UserPreferencesCategories");

            migrationBuilder.DropForeignKey(
                name: "FK_UserPreferencesCategories_UserPreferences_UserPreferencesId",
                table: "UserPreferencesCategories");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserPreferencesCategories",
                table: "UserPreferencesCategories");

            migrationBuilder.RenameTable(
                name: "UserPreferencesCategories",
                newName: "UsersPreferences");

            migrationBuilder.RenameIndex(
                name: "IX_UserPreferencesCategories_UserPreferencesId",
                table: "UsersPreferences",
                newName: "IX_UsersPreferences_UserPreferencesId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UsersPreferences",
                table: "UsersPreferences",
                columns: new[] { "CategoriesId", "UserPreferencesId" });

            migrationBuilder.AddForeignKey(
                name: "FK_UsersPreferences_Categories_CategoriesId",
                table: "UsersPreferences",
                column: "CategoriesId",
                principalTable: "Categories",
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
    }
}
