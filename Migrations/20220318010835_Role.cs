using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace _4kTiles_Backend.Migrations
{
    public partial class Role : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "role",
                columns: new[] { "roleid", "rolename" },
                values: new object[,]
                {
                    { 1, "Admin" },
                    { 2, "User" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "role",
                keyColumn: "roleid",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "role",
                keyColumn: "roleid",
                keyValue: 2);
        }
    }
}
