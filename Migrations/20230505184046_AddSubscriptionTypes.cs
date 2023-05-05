using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DatingApp.Migrations
{
    public partial class AddSubscriptionTypes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SubscriptionTypeId",
                table: "VipRequests",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "SubscriptionTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Months = table.Column<int>(type: "int", nullable: false),
                    Price = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubscriptionTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UsersSubscriptions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    SubscriptionTypeId = table.Column<int>(type: "int", nullable: false),
                    CreateDateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ExpireDateTime = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UsersSubscriptions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UsersSubscriptions_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_UsersSubscriptions_SubscriptionTypes_SubscriptionTypeId",
                        column: x => x.SubscriptionTypeId,
                        principalTable: "SubscriptionTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "SubscriptionTypes",
                columns: new[] { "Id", "Description", "Months", "Name", "Price" },
                values: new object[] { 1, "Monthly subscription to renew each month", 1, "Monthly", 4.99 });

            migrationBuilder.InsertData(
                table: "SubscriptionTypes",
                columns: new[] { "Id", "Description", "Months", "Name", "Price" },
                values: new object[] { 2, "Quarterly subscription to renew each quarter", 3, "Quarterly", 9.99 });

            migrationBuilder.InsertData(
                table: "SubscriptionTypes",
                columns: new[] { "Id", "Description", "Months", "Name", "Price" },
                values: new object[] { 3, "Monthly subscription to renew each six months", 6, "Half-year", 14.99 });

            migrationBuilder.CreateIndex(
                name: "IX_VipRequests_SubscriptionTypeId",
                table: "VipRequests",
                column: "SubscriptionTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_UsersSubscriptions_SubscriptionTypeId",
                table: "UsersSubscriptions",
                column: "SubscriptionTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_UsersSubscriptions_UserId",
                table: "UsersSubscriptions",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_VipRequests_SubscriptionTypes_SubscriptionTypeId",
                table: "VipRequests",
                column: "SubscriptionTypeId",
                principalTable: "SubscriptionTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_VipRequests_SubscriptionTypes_SubscriptionTypeId",
                table: "VipRequests");

            migrationBuilder.DropTable(
                name: "UsersSubscriptions");

            migrationBuilder.DropTable(
                name: "SubscriptionTypes");

            migrationBuilder.DropIndex(
                name: "IX_VipRequests_SubscriptionTypeId",
                table: "VipRequests");

            migrationBuilder.DropColumn(
                name: "SubscriptionTypeId",
                table: "VipRequests");
        }
    }
}
