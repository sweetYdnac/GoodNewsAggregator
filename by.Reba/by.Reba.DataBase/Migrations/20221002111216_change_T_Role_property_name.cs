using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace by.Reba.DataBase.Migrations
{
    public partial class change_T_Role_property_name : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Title",
                table: "Roles",
                newName: "Name");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Roles",
                newName: "Title");
        }
    }
}
