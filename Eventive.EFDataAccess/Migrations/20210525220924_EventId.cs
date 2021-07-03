using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Eventive.EFDataAccess.Migrations
{
    public partial class EventId : Migration
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
                name: "EventOrganizedId",
                table: "Ratings");

            migrationBuilder.AddColumn<Guid>(
                name: "EventId",
                table: "Ratings",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "EventOrganizedId",
                table: "Interactions",
                nullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "EventOrganizedId",
                table: "Comments",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddColumn<Guid>(
                name: "EventId",
                table: "Comments",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AlterColumn<Guid>(
                name: "EventOrganizedId",
                table: "Applications",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddColumn<Guid>(
                name: "EventId",
                table: "Applications",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Interactions_EventOrganizedId",
                table: "Interactions",
                column: "EventOrganizedId");

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

            migrationBuilder.AddForeignKey(
                name: "FK_Interactions_Events_EventOrganizedId",
                table: "Interactions",
                column: "EventOrganizedId",
                principalTable: "Events",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Applications_Events_EventOrganizedId",
                table: "Applications");

            migrationBuilder.DropForeignKey(
                name: "FK_Comments_Events_EventOrganizedId",
                table: "Comments");

            migrationBuilder.DropForeignKey(
                name: "FK_Interactions_Events_EventOrganizedId",
                table: "Interactions");

            migrationBuilder.DropIndex(
                name: "IX_Interactions_EventOrganizedId",
                table: "Interactions");

            migrationBuilder.DropColumn(
                name: "EventId",
                table: "Ratings");

            migrationBuilder.DropColumn(
                name: "EventOrganizedId",
                table: "Interactions");

            migrationBuilder.DropColumn(
                name: "EventId",
                table: "Comments");

            migrationBuilder.DropColumn(
                name: "EventId",
                table: "Applications");

            migrationBuilder.AddColumn<Guid>(
                name: "EventOrganizedId",
                table: "Ratings",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AlterColumn<Guid>(
                name: "EventOrganizedId",
                table: "Comments",
                type: "uniqueidentifier",
                nullable: false,
                oldClrType: typeof(Guid),
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "EventOrganizedId",
                table: "Applications",
                type: "uniqueidentifier",
                nullable: false,
                oldClrType: typeof(Guid),
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
    }
}
