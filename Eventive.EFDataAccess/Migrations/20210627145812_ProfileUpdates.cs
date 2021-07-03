using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Eventive.EFDataAccess.Migrations
{
    public partial class ProfileUpdates : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DateOfBirth",
                table: "Participants");

            migrationBuilder.AddColumn<int>(
                name: "Age",
                table: "Participants",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Participants",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Age",
                table: "Participants");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "Participants");

            migrationBuilder.AddColumn<DateTime>(
                name: "DateOfBirth",
                table: "Participants",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }
    }
}
