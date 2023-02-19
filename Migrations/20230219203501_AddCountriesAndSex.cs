using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DatingApp.Migrations
{
    public partial class AddCountriesAndSex : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Cities",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Cities",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Cities",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Cities",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Cities",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DropColumn(
                name: "Sex",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "SexPreferences",
                table: "AspNetUsers");

            migrationBuilder.AddColumn<string>(
                name: "CountryCode",
                table: "Cities",
                type: "nvarchar(2)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "SexId",
                table: "AspNetUsers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "SexPreferencesId",
                table: "AspNetUsers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Countries",
                columns: table => new
                {
                    Code = table.Column<string>(type: "nvarchar(2)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Countries", x => x.Code);
                });

            migrationBuilder.CreateTable(
                name: "Sex",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sex", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Sex",
                columns: new[] { "Id", "Name" },
                values: new object[] { 1, "Not Mentioned" });

            migrationBuilder.InsertData(
                table: "Sex",
                columns: new[] { "Id", "Name" },
                values: new object[] { 2, "Male" });

            migrationBuilder.InsertData(
                table: "Sex",
                columns: new[] { "Id", "Name" },
                values: new object[] { 3, "Female" });

            migrationBuilder.CreateIndex(
                name: "IX_Cities_CountryCode",
                table: "Cities",
                column: "CountryCode");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_SexId",
                table: "AspNetUsers",
                column: "SexId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_SexPreferencesId",
                table: "AspNetUsers",
                column: "SexPreferencesId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Sex_SexId",
                table: "AspNetUsers",
                column: "SexId",
                principalTable: "Sex",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Sex_SexPreferencesId",
                table: "AspNetUsers",
                column: "SexPreferencesId",
                principalTable: "Sex",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Cities_Countries_CountryCode",
                table: "Cities",
                column: "CountryCode",
                principalTable: "Countries",
                principalColumn: "Code",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Sex_SexId",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Sex_SexPreferencesId",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_Cities_Countries_CountryCode",
                table: "Cities");

            migrationBuilder.DropTable(
                name: "Countries");

            migrationBuilder.DropTable(
                name: "Sex");

            migrationBuilder.DropIndex(
                name: "IX_Cities_CountryCode",
                table: "Cities");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_SexId",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_SexPreferencesId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "CountryCode",
                table: "Cities");

            migrationBuilder.DropColumn(
                name: "SexId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "SexPreferencesId",
                table: "AspNetUsers");

            migrationBuilder.AddColumn<byte>(
                name: "Sex",
                table: "AspNetUsers",
                type: "tinyint",
                nullable: false,
                defaultValue: (byte)0);

            migrationBuilder.AddColumn<byte>(
                name: "SexPreferences",
                table: "AspNetUsers",
                type: "tinyint",
                nullable: false,
                defaultValue: (byte)0);

            migrationBuilder.InsertData(
                table: "Cities",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Київ" },
                    { 2, "Львів" },
                    { 3, "Івано-Франківськ" },
                    { 4, "Луцьк" },
                    { 5, "Тернопіль" }
                });
        }
    }
}
