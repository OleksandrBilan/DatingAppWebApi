using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DatingApp.Migrations
{
    public partial class RemoveUserSexPreferences : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Sex_SexPreferencesId",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_SexPreferencesId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "SexPreferencesId",
                table: "AspNetUsers");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SexPreferencesId",
                table: "AspNetUsers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_SexPreferencesId",
                table: "AspNetUsers",
                column: "SexPreferencesId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Sex_SexPreferencesId",
                table: "AspNetUsers",
                column: "SexPreferencesId",
                principalTable: "Sex",
                principalColumn: "Id");
        }
    }
}
