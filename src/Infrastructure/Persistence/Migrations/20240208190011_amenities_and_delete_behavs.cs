using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class amenitiesanddeletebehavs : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Rooms_RoomInfos_RoomInfoId",
                table: "Rooms");


            migrationBuilder.AlterColumn<Guid>(
                name: "RoomInfoId",
                table: "Rooms",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.CreateTable(
                name: "Amenities",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    HotelId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Amenities", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Amenities_Hotels_HotelId",
                        column: x => x.HotelId,
                        principalTable: "Hotels",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });


            migrationBuilder.CreateIndex(
                name: "IX_Amenities_HotelId",
                table: "Amenities",
                column: "HotelId");

            migrationBuilder.AddForeignKey(
                name: "FK_Rooms_RoomInfos_RoomInfoId",
                table: "Rooms",
                column: "RoomInfoId",
                principalTable: "RoomInfos",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Rooms_RoomInfos_RoomInfoId",
                table: "Rooms");

  

            migrationBuilder.DropTable(
                name: "Amenities");

            migrationBuilder.DropColumn(
                name: "RoomInfoId1",
                table: "Rooms");

            migrationBuilder.AlterColumn<Guid>(
                name: "RoomInfoId",
                table: "Rooms",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Rooms_RoomInfos_RoomInfoId",
                table: "Rooms",
                column: "RoomInfoId",
                principalTable: "RoomInfos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
