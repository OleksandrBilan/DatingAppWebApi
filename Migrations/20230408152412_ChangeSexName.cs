using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DatingApp.Migrations
{
    public partial class ChangeSexName : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Sex",
                keyColumn: "Id",
                keyValue: 1,
                column: "Name",
                value: "Else");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Sex",
                keyColumn: "Id",
                keyValue: 1,
                column: "Name",
                value: "Not Mentioned");
        }
    }
}
