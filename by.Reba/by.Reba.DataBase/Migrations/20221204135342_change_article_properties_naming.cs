using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace by.Reba.DataBase.Migrations
{
    public partial class change_article_properties_naming : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Articles_Positivities_RatingId",
                table: "Articles");

            migrationBuilder.RenameColumn(
                name: "RatingId",
                table: "Articles",
                newName: "PositivityId");

            migrationBuilder.RenameIndex(
                name: "IX_Articles_RatingId",
                table: "Articles",
                newName: "IX_Articles_PositivityId");

            migrationBuilder.AddForeignKey(
                name: "FK_Articles_Positivities_PositivityId",
                table: "Articles",
                column: "PositivityId",
                principalTable: "Positivities",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Articles_Positivities_PositivityId",
                table: "Articles");

            migrationBuilder.RenameColumn(
                name: "PositivityId",
                table: "Articles",
                newName: "RatingId");

            migrationBuilder.RenameIndex(
                name: "IX_Articles_PositivityId",
                table: "Articles",
                newName: "IX_Articles_RatingId");

            migrationBuilder.AddForeignKey(
                name: "FK_Articles_Positivities_RatingId",
                table: "Articles",
                column: "RatingId",
                principalTable: "Positivities",
                principalColumn: "Id");
        }
    }
}
