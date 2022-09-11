using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace by.Reba.DataBase.Migrations
{
    public partial class ChangeArticlePropertyName : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CreationTime",
                table: "Articles",
                newName: "PublicationDate");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PublicationDate",
                table: "Articles",
                newName: "CreationTime");
        }
    }
}
