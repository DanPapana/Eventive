using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Eventive.EFDataAccess.Migrations
{
    public partial class @byte : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte[]>(
                name: "ProfileImageByteArray",
                table: "Users",
                nullable: true);

            migrationBuilder.AddColumn<byte[]>(
                name: "ImageByteArray",
                table: "Events",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProfileImageByteArray",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "ImageByteArray",
                table: "Events");
        }
    }
}
