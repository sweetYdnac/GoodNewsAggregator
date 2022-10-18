using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace by.Reba.DataBase.Migrations
{
    public partial class change_assessment_logic : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Assessment",
                table: "Comments");

            migrationBuilder.DropColumn(
                name: "Assessment",
                table: "Articles");

            migrationBuilder.CreateTable(
                name: "UsersNegativeArticles",
                columns: table => new
                {
                    NegativeArticlesId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UsersWithNegativeAssessmentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UsersNegativeArticles", x => new { x.NegativeArticlesId, x.UsersWithNegativeAssessmentId });
                    table.ForeignKey(
                        name: "FK_UsersNegativeArticles_Articles_NegativeArticlesId",
                        column: x => x.NegativeArticlesId,
                        principalTable: "Articles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UsersNegativeArticles_Users_UsersWithNegativeAssessmentId",
                        column: x => x.UsersWithNegativeAssessmentId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UsersNegativeComments",
                columns: table => new
                {
                    NegativeCommentsId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UsersWithNegativeAssessmentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UsersNegativeComments", x => new { x.NegativeCommentsId, x.UsersWithNegativeAssessmentId });
                    table.ForeignKey(
                        name: "FK_UsersNegativeComments_Comments_NegativeCommentsId",
                        column: x => x.NegativeCommentsId,
                        principalTable: "Comments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UsersNegativeComments_Users_UsersWithNegativeAssessmentId",
                        column: x => x.UsersWithNegativeAssessmentId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UsersPositiveArticles",
                columns: table => new
                {
                    PositiveArticlesId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UsersWithPositiveAssessmentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UsersPositiveArticles", x => new { x.PositiveArticlesId, x.UsersWithPositiveAssessmentId });
                    table.ForeignKey(
                        name: "FK_UsersPositiveArticles_Articles_PositiveArticlesId",
                        column: x => x.PositiveArticlesId,
                        principalTable: "Articles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UsersPositiveArticles_Users_UsersWithPositiveAssessmentId",
                        column: x => x.UsersWithPositiveAssessmentId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UsersPositiveComments",
                columns: table => new
                {
                    PositiveCommentsId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UsersWithPositiveAssessmentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UsersPositiveComments", x => new { x.PositiveCommentsId, x.UsersWithPositiveAssessmentId });
                    table.ForeignKey(
                        name: "FK_UsersPositiveComments_Comments_PositiveCommentsId",
                        column: x => x.PositiveCommentsId,
                        principalTable: "Comments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UsersPositiveComments_Users_UsersWithPositiveAssessmentId",
                        column: x => x.UsersWithPositiveAssessmentId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UsersNegativeArticles_UsersWithNegativeAssessmentId",
                table: "UsersNegativeArticles",
                column: "UsersWithNegativeAssessmentId");

            migrationBuilder.CreateIndex(
                name: "IX_UsersNegativeComments_UsersWithNegativeAssessmentId",
                table: "UsersNegativeComments",
                column: "UsersWithNegativeAssessmentId");

            migrationBuilder.CreateIndex(
                name: "IX_UsersPositiveArticles_UsersWithPositiveAssessmentId",
                table: "UsersPositiveArticles",
                column: "UsersWithPositiveAssessmentId");

            migrationBuilder.CreateIndex(
                name: "IX_UsersPositiveComments_UsersWithPositiveAssessmentId",
                table: "UsersPositiveComments",
                column: "UsersWithPositiveAssessmentId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UsersNegativeArticles");

            migrationBuilder.DropTable(
                name: "UsersNegativeComments");

            migrationBuilder.DropTable(
                name: "UsersPositiveArticles");

            migrationBuilder.DropTable(
                name: "UsersPositiveComments");

            migrationBuilder.AddColumn<int>(
                name: "Assessment",
                table: "Comments",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Assessment",
                table: "Articles",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
