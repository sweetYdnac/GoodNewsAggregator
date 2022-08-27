using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace by.Reba.API.Migrations
{
    public partial class FixHistoryAndBookmarksJoinTablesNames : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_T_ArticleT_User_Articles_BookmarksId",
                table: "T_ArticleT_User");

            migrationBuilder.DropForeignKey(
                name: "FK_T_ArticleT_User_Users_UserBookmarksId",
                table: "T_ArticleT_User");

            migrationBuilder.DropForeignKey(
                name: "FK_T_ArticleT_User1_Articles_HistoryId",
                table: "T_ArticleT_User1");

            migrationBuilder.DropForeignKey(
                name: "FK_T_ArticleT_User1_Users_UserHistoryId",
                table: "T_ArticleT_User1");

            migrationBuilder.DropPrimaryKey(
                name: "PK_T_ArticleT_User1",
                table: "T_ArticleT_User1");

            migrationBuilder.DropPrimaryKey(
                name: "PK_T_ArticleT_User",
                table: "T_ArticleT_User");

            migrationBuilder.RenameTable(
                name: "T_ArticleT_User1",
                newName: "History");

            migrationBuilder.RenameTable(
                name: "T_ArticleT_User",
                newName: "Bookmarks");

            migrationBuilder.RenameIndex(
                name: "IX_T_ArticleT_User1_UserHistoryId",
                table: "History",
                newName: "IX_History_UserHistoryId");

            migrationBuilder.RenameIndex(
                name: "IX_T_ArticleT_User_UserBookmarksId",
                table: "Bookmarks",
                newName: "IX_Bookmarks_UserBookmarksId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_History",
                table: "History",
                columns: new[] { "HistoryId", "UserHistoryId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_Bookmarks",
                table: "Bookmarks",
                columns: new[] { "BookmarksId", "UserBookmarksId" });

            migrationBuilder.AddForeignKey(
                name: "FK_Bookmarks_Articles_BookmarksId",
                table: "Bookmarks",
                column: "BookmarksId",
                principalTable: "Articles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Bookmarks_Users_UserBookmarksId",
                table: "Bookmarks",
                column: "UserBookmarksId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_History_Articles_HistoryId",
                table: "History",
                column: "HistoryId",
                principalTable: "Articles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_History_Users_UserHistoryId",
                table: "History",
                column: "UserHistoryId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bookmarks_Articles_BookmarksId",
                table: "Bookmarks");

            migrationBuilder.DropForeignKey(
                name: "FK_Bookmarks_Users_UserBookmarksId",
                table: "Bookmarks");

            migrationBuilder.DropForeignKey(
                name: "FK_History_Articles_HistoryId",
                table: "History");

            migrationBuilder.DropForeignKey(
                name: "FK_History_Users_UserHistoryId",
                table: "History");

            migrationBuilder.DropPrimaryKey(
                name: "PK_History",
                table: "History");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Bookmarks",
                table: "Bookmarks");

            migrationBuilder.RenameTable(
                name: "History",
                newName: "T_ArticleT_User1");

            migrationBuilder.RenameTable(
                name: "Bookmarks",
                newName: "T_ArticleT_User");

            migrationBuilder.RenameIndex(
                name: "IX_History_UserHistoryId",
                table: "T_ArticleT_User1",
                newName: "IX_T_ArticleT_User1_UserHistoryId");

            migrationBuilder.RenameIndex(
                name: "IX_Bookmarks_UserBookmarksId",
                table: "T_ArticleT_User",
                newName: "IX_T_ArticleT_User_UserBookmarksId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_T_ArticleT_User1",
                table: "T_ArticleT_User1",
                columns: new[] { "HistoryId", "UserHistoryId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_T_ArticleT_User",
                table: "T_ArticleT_User",
                columns: new[] { "BookmarksId", "UserBookmarksId" });

            migrationBuilder.AddForeignKey(
                name: "FK_T_ArticleT_User_Articles_BookmarksId",
                table: "T_ArticleT_User",
                column: "BookmarksId",
                principalTable: "Articles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_T_ArticleT_User_Users_UserBookmarksId",
                table: "T_ArticleT_User",
                column: "UserBookmarksId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_T_ArticleT_User1_Articles_HistoryId",
                table: "T_ArticleT_User1",
                column: "HistoryId",
                principalTable: "Articles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_T_ArticleT_User1_Users_UserHistoryId",
                table: "T_ArticleT_User1",
                column: "UserHistoryId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
