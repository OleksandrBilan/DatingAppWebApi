using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DatingApp.Migrations
{
    public partial class AddMessageStatuses : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "StatusId",
                table: "Messages",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "MessagesStatuses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MessagesStatuses", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Messages_StatusId",
                table: "Messages",
                column: "StatusId");

            migrationBuilder.AddForeignKey(
                name: "FK_Messages_MessagesStatuses_StatusId",
                table: "Messages",
                column: "StatusId",
                principalTable: "MessagesStatuses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Messages_MessagesStatuses_StatusId",
                table: "Messages");

            migrationBuilder.DropTable(
                name: "MessagesStatuses");

            migrationBuilder.DropIndex(
                name: "IX_Messages_StatusId",
                table: "Messages");

            migrationBuilder.DropColumn(
                name: "StatusId",
                table: "Messages");
        }
    }
}
