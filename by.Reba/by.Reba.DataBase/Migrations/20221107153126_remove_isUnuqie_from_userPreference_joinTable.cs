using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace by.Reba.DataBase.Migrations
{
    public partial class remove_isUnuqie_from_userPreference_joinTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(name: "IX_UserPreferences_PositivityRatingId", table: "UserPreferences");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_UserPreferences_PositivityRatingId",
                table: "UserPreferences",
                column: "PositivityRatingId",
                unique: true
                );
        }
    }
}
