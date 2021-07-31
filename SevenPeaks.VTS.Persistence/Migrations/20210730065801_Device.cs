using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SevenPeaks.VTS.Persistence.Migrations
{
    public partial class Device : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DeviceId",
                table: "Vehicles",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Device",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Device", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Vehicles_DeviceId",
                table: "Vehicles",
                column: "DeviceId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Vehicles_Device_DeviceId",
                table: "Vehicles",
                column: "DeviceId",
                principalTable: "Device",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Vehicles_Device_DeviceId",
                table: "Vehicles");

            migrationBuilder.DropTable(
                name: "Device");

            migrationBuilder.DropIndex(
                name: "IX_Vehicles_DeviceId",
                table: "Vehicles");

            migrationBuilder.DropColumn(
                name: "DeviceId",
                table: "Vehicles");
        }
    }
}
