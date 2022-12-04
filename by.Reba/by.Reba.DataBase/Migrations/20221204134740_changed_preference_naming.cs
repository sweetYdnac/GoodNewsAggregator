using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace by.Reba.DataBase.Migrations
{
    public partial class changed_preference_naming : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Preferences_Positivities_PositivityRatingId",
                table: "Preferences");

            migrationBuilder.RenameColumn(
                name: "PositivityRatingId",
                table: "Preferences",
                newName: "MinPositivityId");

            migrationBuilder.AddForeignKey(
                name: "FK_Preferences_Positivities_MinPositivityId",
                table: "Preferences",
                column: "MinPositivityId",
                principalTable: "Positivities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Preferences_Positivities_MinPositivityId",
                table: "Preferences");

            migrationBuilder.RenameColumn(
                name: "MinPositivityId",
                table: "Preferences",
                newName: "PositivityRatingId");

            migrationBuilder.AddForeignKey(
                name: "FK_Preferences_Positivities_PositivityRatingId",
                table: "Preferences",
                column: "PositivityRatingId",
                principalTable: "Positivities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
