using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Eventive.EFDataAccess.Migrations
{
    public partial class Applications : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comments_Events_OrganizedEventId",
                table: "Comments");

            migrationBuilder.DropIndex(
                name: "IX_Comments_OrganizedEventId",
                table: "Comments");

            migrationBuilder.AddColumn<bool>(
                name: "ApplicationRequired",
                table: "EventDetails",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<Guid>(
                name: "EventOrganizedId",
                table: "Comments",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Applications",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    EventId = table.Column<Guid>(nullable: false),
                    ParticipantId = table.Column<Guid>(nullable: false),
                    ApplicationText = table.Column<string>(nullable: true),
                    Timestamp = table.Column<DateTime>(nullable: false),
                    EventOrganizedId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Applications", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Applications_Events_EventOrganizedId",
                        column: x => x.EventOrganizedId,
                        principalTable: "Events",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Comments_EventOrganizedId",
                table: "Comments",
                column: "EventOrganizedId");

            migrationBuilder.CreateIndex(
                name: "IX_Applications_EventOrganizedId",
                table: "Applications",
                column: "EventOrganizedId");

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_Events_EventOrganizedId",
                table: "Comments",
                column: "EventOrganizedId",
                principalTable: "Events",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comments_Events_EventOrganizedId",
                table: "Comments");

            migrationBuilder.DropTable(
                name: "Applications");

            migrationBuilder.DropIndex(
                name: "IX_Comments_EventOrganizedId",
                table: "Comments");

            migrationBuilder.DropColumn(
                name: "ApplicationRequired",
                table: "EventDetails");

            migrationBuilder.DropColumn(
                name: "EventOrganizedId",
                table: "Comments");

            migrationBuilder.CreateIndex(
                name: "IX_Comments_OrganizedEventId",
                table: "Comments",
                column: "OrganizedEventId");

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_Events_OrganizedEventId",
                table: "Comments",
                column: "OrganizedEventId",
                principalTable: "Events",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
