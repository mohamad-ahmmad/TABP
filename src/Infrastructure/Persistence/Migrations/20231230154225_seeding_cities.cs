using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class seedingcities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Cities",
                columns: new[] { "Id", "CityName", "CountryName", "Created", "CreatedBy", "IsDeleted", "LastModified", "LastModifiedBy", "ThumbnailUrl" },
                values: new object[,]
                {
                    { new Guid("5beac0db-93ba-4ebe-86e2-f29f577995a2"), "Japan", "Tokyo", DateTime.UtcNow, new Guid("00000000-0000-0000-0000-000000000000"), false, DateTime.UtcNow, new Guid("00000000-0000-0000-0000-000000000000"), "1.jpg" },
                    { new Guid("d55a1cac-c04b-41cf-8924-b9a4d0d95cd0"), "Moscow", "Russia", DateTime.UtcNow, new Guid("00000000-0000-0000-0000-000000000000"), false, DateTime.UtcNow, new Guid("00000000-0000-0000-0000-000000000000"), "2.jpg" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Cities",
                keyColumn: "Id",
                keyValue: new Guid("5beac0db-93ba-4ebe-86e2-f29f577995a2"));

            migrationBuilder.DeleteData(
                table: "Cities",
                keyColumn: "Id",
                keyValue: new Guid("d55a1cac-c04b-41cf-8924-b9a4d0d95cd0"));
        }
    }
}
