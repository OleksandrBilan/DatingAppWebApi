using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DatingApp.Migrations
{
    public partial class UpdateSubscriptionTypesInfo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "SubscriptionTypes",
                keyColumn: "Id",
                keyValue: 3,
                column: "Description",
                value: "Half-year subscription to renew each six months");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "SubscriptionTypes",
                keyColumn: "Id",
                keyValue: 3,
                column: "Description",
                value: "Monthly subscription to renew each six months");
        }
    }
}
