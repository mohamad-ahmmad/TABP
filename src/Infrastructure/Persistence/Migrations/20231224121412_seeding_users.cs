using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class seedingusers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Email", "Firstname", "IsDeleted", "Lastname", "Password", "UserLevel", "Username" },
                values: new object[,]
                {
                    { new Guid("8c667e48-0b4f-49a8-a964-9c44698bc860"), "ml7m@gmail.com", "Melheem", false, "Met'b", "5e884898da28047151d0e56f8dc6292773603d0d6aabbdd62a11ef721d1542d8", 1, "ml7m" },
                    { new Guid("c3fea012-2148-41b4-9b76-b6a30293bf5d"), "mail@gmail.com", "Mohammad", false, "Ahmad", "5e884898da28047151d0e56f8dc6292773603d0d6aabbdd62a11ef721d1542d8", 1, "mohah" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("8c667e48-0b4f-49a8-a964-9c44698bc860"));

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("c3fea012-2148-41b4-9b76-b6a30293bf5d"));
        }
    }
}
