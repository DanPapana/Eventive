using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Eventive.EFDataAccess.Migrations
{
    public partial class RenamingEventRating : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Ratings_Participants_ParticipantId",
                table: "Ratings");

            migrationBuilder.DropColumn(
                name: "EventId",
                table: "Ratings");

            migrationBuilder.AlterColumn<Guid>(
                name: "ParticipantId",
                table: "Ratings",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddColumn<Guid>(
                name: "EventOrganizedId",
                table: "Ratings",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Ratings_EventOrganizedId",
                table: "Ratings",
                column: "EventOrganizedId");

            migrationBuilder.AddForeignKey(
                name: "FK_Ratings_Events_EventOrganizedId",
                table: "Ratings",
                column: "EventOrganizedId",
                principalTable: "Events",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Ratings_Participants_ParticipantId",
                table: "Ratings",
                column: "ParticipantId",
                principalTable: "Participants",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Ratings_Events_EventOrganizedId",
                table: "Ratings");

            migrationBuilder.DropForeignKey(
                name: "FK_Ratings_Participants_ParticipantId",
                table: "Ratings");

            migrationBuilder.DropIndex(
                name: "IX_Ratings_EventOrganizedId",
                table: "Ratings");

            migrationBuilder.DropColumn(
                name: "EventOrganizedId",
                table: "Ratings");

            migrationBuilder.AlterColumn<Guid>(
                name: "ParticipantId",
                table: "Ratings",
                type: "uniqueidentifier",
                nullable: false,
                oldClrType: typeof(Guid),
                oldNullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "EventId",
                table: "Ratings",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddForeignKey(
                name: "FK_Ratings_Participants_ParticipantId",
                table: "Ratings",
                column: "ParticipantId",
                principalTable: "Participants",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
