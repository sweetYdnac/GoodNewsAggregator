using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace by.Reba.DataBase.Migrations
{
    public partial class change_history_table_name : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserHistory_Articles_ArticleId",
                table: "UserHistory");

            migrationBuilder.DropForeignKey(
                name: "FK_UserHistory_Users_UserId",
                table: "UserHistory");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserHistory",
                table: "UserHistory");

            migrationBuilder.RenameTable(
                name: "UserHistory",
                newName: "Histories");

            migrationBuilder.RenameIndex(
                name: "IX_UserHistory_UserId",
                table: "Histories",
                newName: "IX_Histories_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_UserHistory_ArticleId",
                table: "Histories",
                newName: "IX_Histories_ArticleId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Histories",
                table: "Histories",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Histories_Articles_ArticleId",
                table: "Histories",
                column: "ArticleId",
                principalTable: "Articles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Histories_Users_UserId",
                table: "Histories",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Histories_Articles_ArticleId",
                table: "Histories");

            migrationBuilder.DropForeignKey(
                name: "FK_Histories_Users_UserId",
                table: "Histories");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Histories",
                table: "Histories");

            migrationBuilder.RenameTable(
                name: "Histories",
                newName: "UserHistory");

            migrationBuilder.RenameIndex(
                name: "IX_Histories_UserId",
                table: "UserHistory",
                newName: "IX_UserHistory_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Histories_ArticleId",
                table: "UserHistory",
                newName: "IX_UserHistory_ArticleId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserHistory",
                table: "UserHistory",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UserHistory_Articles_ArticleId",
                table: "UserHistory",
                column: "ArticleId",
                principalTable: "Articles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserHistory_Users_UserId",
                table: "UserHistory",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
