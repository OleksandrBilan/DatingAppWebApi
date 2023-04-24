using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DatingApp.Migrations
{
    public partial class AddLikesIds : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MutualLikes_AspNetUsers_User1Id",
                table: "MutualLikes");

            migrationBuilder.DropForeignKey(
                name: "FK_MutualLikes_AspNetUsers_User2Id",
                table: "MutualLikes");

            migrationBuilder.DropForeignKey(
                name: "FK_UsersLikes_AspNetUsers_LikedUserId",
                table: "UsersLikes");

            migrationBuilder.DropForeignKey(
                name: "FK_UsersLikes_AspNetUsers_LikingUserId",
                table: "UsersLikes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UsersLikes",
                table: "UsersLikes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MutualLikes",
                table: "MutualLikes");

            migrationBuilder.AlterColumn<string>(
                name: "LikedUserId",
                table: "UsersLikes",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "LikingUserId",
                table: "UsersLikes",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "UsersLikes",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AlterColumn<string>(
                name: "User2Id",
                table: "MutualLikes",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "User1Id",
                table: "MutualLikes",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "MutualLikes",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UsersLikes",
                table: "UsersLikes",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_MutualLikes",
                table: "MutualLikes",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_UsersLikes_LikingUserId",
                table: "UsersLikes",
                column: "LikingUserId");

            migrationBuilder.CreateIndex(
                name: "IX_MutualLikes_User1Id",
                table: "MutualLikes",
                column: "User1Id");

            migrationBuilder.AddForeignKey(
                name: "FK_MutualLikes_AspNetUsers_User1Id",
                table: "MutualLikes",
                column: "User1Id",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_MutualLikes_AspNetUsers_User2Id",
                table: "MutualLikes",
                column: "User2Id",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UsersLikes_AspNetUsers_LikedUserId",
                table: "UsersLikes",
                column: "LikedUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UsersLikes_AspNetUsers_LikingUserId",
                table: "UsersLikes",
                column: "LikingUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MutualLikes_AspNetUsers_User1Id",
                table: "MutualLikes");

            migrationBuilder.DropForeignKey(
                name: "FK_MutualLikes_AspNetUsers_User2Id",
                table: "MutualLikes");

            migrationBuilder.DropForeignKey(
                name: "FK_UsersLikes_AspNetUsers_LikedUserId",
                table: "UsersLikes");

            migrationBuilder.DropForeignKey(
                name: "FK_UsersLikes_AspNetUsers_LikingUserId",
                table: "UsersLikes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UsersLikes",
                table: "UsersLikes");

            migrationBuilder.DropIndex(
                name: "IX_UsersLikes_LikingUserId",
                table: "UsersLikes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MutualLikes",
                table: "MutualLikes");

            migrationBuilder.DropIndex(
                name: "IX_MutualLikes_User1Id",
                table: "MutualLikes");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "UsersLikes");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "MutualLikes");

            migrationBuilder.AlterColumn<string>(
                name: "LikingUserId",
                table: "UsersLikes",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "LikedUserId",
                table: "UsersLikes",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "User2Id",
                table: "MutualLikes",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "User1Id",
                table: "MutualLikes",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_UsersLikes",
                table: "UsersLikes",
                columns: new[] { "LikingUserId", "LikedUserId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_MutualLikes",
                table: "MutualLikes",
                columns: new[] { "User1Id", "User2Id" });

            migrationBuilder.AddForeignKey(
                name: "FK_MutualLikes_AspNetUsers_User1Id",
                table: "MutualLikes",
                column: "User1Id",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MutualLikes_AspNetUsers_User2Id",
                table: "MutualLikes",
                column: "User2Id",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UsersLikes_AspNetUsers_LikedUserId",
                table: "UsersLikes",
                column: "LikedUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UsersLikes_AspNetUsers_LikingUserId",
                table: "UsersLikes",
                column: "LikingUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
