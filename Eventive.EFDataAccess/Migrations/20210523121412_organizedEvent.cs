using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Eventive.EFDataAccess.Migrations
{
    public partial class organizedEvent : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comment_Events_EventId",
                table: "Comment");

            migrationBuilder.DropIndex(
                name: "IX_Comment_EventId",
                table: "Comment");

            migrationBuilder.DropColumn(
                name: "EventId",
                table: "Comment");

            migrationBuilder.AlterColumn<decimal>(
                name: "ParticipationFee",
                table: "EventDetails",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,4)");

            migrationBuilder.AddColumn<Guid>(
                name: "OrganizedEventId",
                table: "Comment",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Comment_OrganizedEventId",
                table: "Comment",
                column: "OrganizedEventId");

            migrationBuilder.AddForeignKey(
                name: "FK_Comment_Events_OrganizedEventId",
                table: "Comment",
                column: "OrganizedEventId",
                principalTable: "Events",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comment_Events_OrganizedEventId",
                table: "Comment");

            migrationBuilder.DropIndex(
                name: "IX_Comment_OrganizedEventId",
                table: "Comment");

            migrationBuilder.DropColumn(
                name: "OrganizedEventId",
                table: "Comment");

            migrationBuilder.AlterColumn<decimal>(
                name: "ParticipationFee",
                table: "EventDetails",
                type: "decimal(18,4)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AddColumn<Guid>(
                name: "EventId",
                table: "Comment",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Comment_EventId",
                table: "Comment",
                column: "EventId");

            migrationBuilder.AddForeignKey(
                name: "FK_Comment_Events_EventId",
                table: "Comment",
                column: "EventId",
                principalTable: "Events",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
