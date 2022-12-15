using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NewsAPI.DataAccess.Migrations
{
    public partial class updateAcessToken : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AcessToken",
                table: "Users",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 10001,
                column: "AcessToken",
                value: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AcessToken",
                table: "Users");
        }
    }
}
