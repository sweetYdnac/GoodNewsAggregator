using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace by.Reba.DataBase.Migrations
{
    public partial class change_positivity_table_name : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Articles_PositivityRatings_RatingId",
                table: "Articles");

            migrationBuilder.DropForeignKey(
                name: "FK_UserPreferences_PositivityRatings_PositivityRatingId",
                table: "UserPreferences");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PositivityRatings",
                table: "PositivityRatings");

            migrationBuilder.RenameTable(
                name: "PositivityRatings",
                newName: "Positivities");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Positivities",
                table: "Positivities",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Articles_Positivities_RatingId",
                table: "Articles",
                column: "RatingId",
                principalTable: "Positivities",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UserPreferences_Positivities_PositivityRatingId",
                table: "UserPreferences",
                column: "PositivityRatingId",
                principalTable: "Positivities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Articles_Positivities_RatingId",
                table: "Articles");

            migrationBuilder.DropForeignKey(
                name: "FK_UserPreferences_Positivities_PositivityRatingId",
                table: "UserPreferences");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Positivities",
                table: "Positivities");

            migrationBuilder.RenameTable(
                name: "Positivities",
                newName: "PositivityRatings");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PositivityRatings",
                table: "PositivityRatings",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Articles_PositivityRatings_RatingId",
                table: "Articles",
                column: "RatingId",
                principalTable: "PositivityRatings",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UserPreferences_PositivityRatings_PositivityRatingId",
                table: "UserPreferences",
                column: "PositivityRatingId",
                principalTable: "PositivityRatings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
