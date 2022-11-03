using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace by.Reba.DataBase.Migrations
{
    public partial class AddLastVisitTimeToCompositKey : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_UserHistory",
                table: "UserHistory");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserHistory",
                table: "UserHistory",
                columns: new[] { "UserId", "ArticleId", "LastVisitTime" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_UserHistory",
                table: "UserHistory");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserHistory",
                table: "UserHistory",
                columns: new[] { "UserId", "ArticleId" });
        }
    }
}
