using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class naviproproominfo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "HotelId",
                table: "RoomInfos",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_RoomInfos_HotelId",
                table: "RoomInfos",
                column: "HotelId");

            migrationBuilder.AddForeignKey(
                name: "FK_RoomInfos_Hotels_HotelId",
                table: "RoomInfos",
                column: "HotelId",
                principalTable: "Hotels",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RoomInfos_Hotels_HotelId",
                table: "RoomInfos");

            migrationBuilder.DropIndex(
                name: "IX_RoomInfos_HotelId",
                table: "RoomInfos");

            migrationBuilder.DropColumn(
                name: "HotelId",
                table: "RoomInfos");

        }
    }
}
