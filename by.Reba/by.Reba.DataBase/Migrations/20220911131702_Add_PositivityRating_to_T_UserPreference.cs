using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace by.Reba.DataBase.Migrations
{
    public partial class Add_PositivityRating_to_T_UserPreference : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserPreferences_Categories_CategoriesId",
                table: "UserPreferences");

            migrationBuilder.DropForeignKey(
                name: "FK_UserPreferences_Preferences_UserPreferencesId",
                table: "UserPreferences");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserPreferences",
                table: "UserPreferences");

            migrationBuilder.RenameTable(
                name: "UserPreferences",
                newName: "UsersPreferences");

            migrationBuilder.RenameIndex(
                name: "IX_UserPreferences_UserPreferencesId",
                table: "UsersPreferences",
                newName: "IX_UsersPreferences_UserPreferencesId");

            migrationBuilder.AddColumn<Guid>(
                name: "PositivityRatingId",
                table: "Preferences",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddPrimaryKey(
                name: "PK_UsersPreferences",
                table: "UsersPreferences",
                columns: new[] { "CategoriesId", "UserPreferencesId" });

            migrationBuilder.CreateIndex(
                name: "IX_Preferences_PositivityRatingId",
                table: "Preferences",
                column: "PositivityRatingId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Preferences_PositivityRatings_PositivityRatingId",
                table: "Preferences",
                column: "PositivityRatingId",
                principalTable: "PositivityRatings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UsersPreferences_Categories_CategoriesId",
                table: "UsersPreferences",
                column: "CategoriesId",
                principalTable: "Categories",
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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Preferences_PositivityRatings_PositivityRatingId",
                table: "Preferences");

            migrationBuilder.DropForeignKey(
                name: "FK_UsersPreferences_Categories_CategoriesId",
                table: "UsersPreferences");

            migrationBuilder.DropForeignKey(
                name: "FK_UsersPreferences_Preferences_UserPreferencesId",
                table: "UsersPreferences");

            migrationBuilder.DropIndex(
                name: "IX_Preferences_PositivityRatingId",
                table: "Preferences");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UsersPreferences",
                table: "UsersPreferences");

            migrationBuilder.DropColumn(
                name: "PositivityRatingId",
                table: "Preferences");

            migrationBuilder.RenameTable(
                name: "UsersPreferences",
                newName: "UserPreferences");

            migrationBuilder.RenameIndex(
                name: "IX_UsersPreferences_UserPreferencesId",
                table: "UserPreferences",
                newName: "IX_UserPreferences_UserPreferencesId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserPreferences",
                table: "UserPreferences",
                columns: new[] { "CategoriesId", "UserPreferencesId" });

            migrationBuilder.AddForeignKey(
                name: "FK_UserPreferences_Categories_CategoriesId",
                table: "UserPreferences",
                column: "CategoriesId",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserPreferences_Preferences_UserPreferencesId",
                table: "UserPreferences",
                column: "UserPreferencesId",
                principalTable: "Preferences",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
