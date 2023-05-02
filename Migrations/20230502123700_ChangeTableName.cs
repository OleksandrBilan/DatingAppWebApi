using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DatingApp.Migrations
{
    public partial class ChangeTableName : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Messages_MessagesStatuses_StatusId",
                table: "Messages");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MessagesStatuses",
                table: "MessagesStatuses");

            migrationBuilder.RenameTable(
                name: "MessagesStatuses",
                newName: "MessageStatuses");

            migrationBuilder.AddPrimaryKey(
                name: "PK_MessageStatuses",
                table: "MessageStatuses",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Messages_MessageStatuses_StatusId",
                table: "Messages",
                column: "StatusId",
                principalTable: "MessageStatuses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Messages_MessageStatuses_StatusId",
                table: "Messages");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MessageStatuses",
                table: "MessageStatuses");

            migrationBuilder.RenameTable(
                name: "MessageStatuses",
                newName: "MessagesStatuses");

            migrationBuilder.AddPrimaryKey(
                name: "PK_MessagesStatuses",
                table: "MessagesStatuses",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Messages_MessagesStatuses_StatusId",
                table: "Messages",
                column: "StatusId",
                principalTable: "MessagesStatuses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
