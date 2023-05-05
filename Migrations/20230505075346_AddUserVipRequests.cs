using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DatingApp.Migrations
{
    public partial class AddUserVipRequests : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "VipRequests",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    AdditionalInfo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreateDateTime = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VipRequests", x => x.Id);
                    table.ForeignKey(
                        name: "FK_VipRequests_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_VipRequests_UserId",
                table: "VipRequests",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "VipRequests");
        }
    }
}
