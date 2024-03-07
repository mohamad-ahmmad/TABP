using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class delete_hotelId_cityId_from_booking : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bookings_Cities_CityId",
                table: "Bookings");

            migrationBuilder.DropForeignKey(
                name: "FK_Bookings_Hotels_HotelId",
                table: "Bookings");

            migrationBuilder.DeleteData(
                table: "Cities",
                keyColumn: "Id",
                keyValue: new Guid("5108bf7b-2579-417b-9174-39fc5370affd"));

            migrationBuilder.DeleteData(
                table: "Cities",
                keyColumn: "Id",
                keyValue: new Guid("5f9aec54-273e-4135-8704-a45c025bcd12"));

            migrationBuilder.InsertData(
                table: "Cities",
                columns: new[] { "Id", "CityName", "CountryName", "Created", "CreatedBy", "IsDeleted", "LastModified", "LastModifiedBy", "Latitude", "Longitude", "PostOfficePostalCode", "ThumbnailUrl" },
                values: new object[,]
                {
                    { new Guid("59a4d8e4-859c-4df0-b5b4-f1a2a5647ca4"), "Moscow", "Russia", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new Guid("00000000-0000-0000-0000-000000000000"), false, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new Guid("00000000-0000-0000-0000-000000000000"), 1.12314, 13.124499999999999, "X32Z", "2.jpg" },
                    { new Guid("dd3eb47b-2bee-44be-8824-caf385ef6fa2"), "Japan", "Tokyo", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new Guid("00000000-0000-0000-0000-000000000000"), false, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new Guid("00000000-0000-0000-0000-000000000000"), 1.12314, 33.124499999999998, "Z32Z", "1.jpg" }
                });

            migrationBuilder.AddForeignKey(
                name: "FK_Bookings_Cities_CityId",
                table: "Bookings",
                column: "CityId",
                principalTable: "Cities",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Bookings_Hotels_HotelId",
                table: "Bookings",
                column: "HotelId",
                principalTable: "Hotels",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bookings_Cities_CityId",
                table: "Bookings");

            migrationBuilder.DropForeignKey(
                name: "FK_Bookings_Hotels_HotelId",
                table: "Bookings");

            migrationBuilder.DeleteData(
                table: "Cities",
                keyColumn: "Id",
                keyValue: new Guid("59a4d8e4-859c-4df0-b5b4-f1a2a5647ca4"));

            migrationBuilder.DeleteData(
                table: "Cities",
                keyColumn: "Id",
                keyValue: new Guid("dd3eb47b-2bee-44be-8824-caf385ef6fa2"));

            migrationBuilder.InsertData(
                table: "Cities",
                columns: new[] { "Id", "CityName", "CountryName", "Created", "CreatedBy", "IsDeleted", "LastModified", "LastModifiedBy", "Latitude", "Longitude", "PostOfficePostalCode", "ThumbnailUrl" },
                values: new object[,]
                {
                    { new Guid("5108bf7b-2579-417b-9174-39fc5370affd"), "Japan", "Tokyo", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new Guid("00000000-0000-0000-0000-000000000000"), false, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new Guid("00000000-0000-0000-0000-000000000000"), 1.12314, 33.124499999999998, "Z32Z", "1.jpg" },
                    { new Guid("5f9aec54-273e-4135-8704-a45c025bcd12"), "Moscow", "Russia", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new Guid("00000000-0000-0000-0000-000000000000"), false, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new Guid("00000000-0000-0000-0000-000000000000"), 1.12314, 13.124499999999999, "X32Z", "2.jpg" }
                });

            migrationBuilder.AddForeignKey(
                name: "FK_Bookings_Cities_CityId",
                table: "Bookings",
                column: "CityId",
                principalTable: "Cities",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Bookings_Hotels_HotelId",
                table: "Bookings",
                column: "HotelId",
                principalTable: "Hotels",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }
    }
}
