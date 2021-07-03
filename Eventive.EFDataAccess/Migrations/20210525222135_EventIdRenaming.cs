using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Eventive.EFDataAccess.Migrations
{
    public partial class EventIdRenaming : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Applications_Events_EventOrganizedId",
                table: "Applications");

            migrationBuilder.DropForeignKey(
                name: "FK_Comments_Events_EventOrganizedId",
                table: "Comments");

            migrationBuilder.DropColumn(
                name: "EventId",
                table: "Comments");

            migrationBuilder.DropColumn(
                name: "EventId",
                table: "Applications");

            migrationBuilder.AlterColumn<Guid>(
                name: "EventOrganizedId",
                table: "Comments",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "EventOrganizedId",
                table: "Applications",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Applications_Events_EventOrganizedId",
                table: "Applications",
                column: "EventOrganizedId",
                principalTable: "Events",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_Events_EventOrganizedId",
                table: "Comments",
                column: "EventOrganizedId",
                principalTable: "Events",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Applications_Events_EventOrganizedId",
                table: "Applications");

            migrationBuilder.DropForeignKey(
                name: "FK_Comments_Events_EventOrganizedId",
                table: "Comments");

            migrationBuilder.AlterColumn<Guid>(
                name: "EventOrganizedId",
                table: "Comments",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid));

            migrationBuilder.AddColumn<Guid>(
                name: "EventId",
                table: "Comments",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AlterColumn<Guid>(
                name: "EventOrganizedId",
                table: "Applications",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid));

            migrationBuilder.AddColumn<Guid>(
                name: "EventId",
                table: "Applications",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddForeignKey(
                name: "FK_Applications_Events_EventOrganizedId",
                table: "Applications",
                column: "EventOrganizedId",
                principalTable: "Events",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_Events_EventOrganizedId",
                table: "Comments",
                column: "EventOrganizedId",
                principalTable: "Events",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
