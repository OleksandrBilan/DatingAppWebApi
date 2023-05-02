using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DatingApp.Migrations
{
    public partial class AddInitialMessageStatuses : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "MessageStatuses",
                columns: new[] { "Id", "Name" },
                values: new object[] { 1, "Sent" });

            migrationBuilder.InsertData(
                table: "MessageStatuses",
                columns: new[] { "Id", "Name" },
                values: new object[] { 2, "Read" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "MessageStatuses",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "MessageStatuses",
                keyColumn: "Id",
                keyValue: 2);
        }
    }
}
