using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SevenPeaks.VTS.Persistence.Migrations
{
    public partial class Cleanup : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Vehicles_Device_DeviceId",
                table: "Vehicles");

            migrationBuilder.DropTable(
                name: "Device");

            migrationBuilder.DropIndex(
                name: "IX_Vehicles_DeviceId",
                table: "Vehicles");

            migrationBuilder.RenameColumn(
                name: "CustomFields",
                table: "Vehicles",
                newName: "Model");

            migrationBuilder.AlterColumn<string>(
                name: "DeviceId",
                table: "Vehicles",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AddColumn<int>(
                name: "Year",
                table: "Vehicles",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Year",
                table: "Vehicles");

            migrationBuilder.RenameColumn(
                name: "Model",
                table: "Vehicles",
                newName: "CustomFields");

            migrationBuilder.AlterColumn<int>(
                name: "DeviceId",
                table: "Vehicles",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "Device",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CreatedDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: true)
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
    }
}
