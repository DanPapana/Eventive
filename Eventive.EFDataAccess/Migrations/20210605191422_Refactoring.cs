using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Eventive.EFDataAccess.Migrations
{
    public partial class Refactoring : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comments_Participants_CommenterId",
                table: "Comments");

            migrationBuilder.DropForeignKey(
                name: "FK_Comments_Events_EventOrganizedId",
                table: "Comments");

            migrationBuilder.DropTable(
                name: "Interactions");

            migrationBuilder.DropIndex(
                name: "IX_Comments_CommenterId",
                table: "Comments");

            migrationBuilder.DropColumn(
                name: "CommenterId",
                table: "Comments");

            migrationBuilder.AlterColumn<Guid>(
                name: "EventOrganizedId",
                table: "Comments",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddColumn<Guid>(
                name: "ParticipantId",
                table: "Comments",
                nullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "ParticipantId",
                table: "Clicks",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<Guid>(
                name: "EventOrganizedId",
                table: "Clicks",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.CreateTable(
                name: "Applications",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    ParticipantId = table.Column<Guid>(nullable: true),
                    EventOrganizedId = table.Column<Guid>(nullable: true),
                    ApplicationText = table.Column<string>(nullable: true)
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
                    table.ForeignKey(
                        name: "FK_Applications_Participants_ParticipantId",
                        column: x => x.ParticipantId,
                        principalTable: "Participants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Followings",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    ParticipantId = table.Column<Guid>(nullable: true),
                    EventOrganizedId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Followings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Followings_Events_EventOrganizedId",
                        column: x => x.EventOrganizedId,
                        principalTable: "Events",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Followings_Participants_ParticipantId",
                        column: x => x.ParticipantId,
                        principalTable: "Participants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Comments_ParticipantId",
                table: "Comments",
                column: "ParticipantId");

            migrationBuilder.CreateIndex(
                name: "IX_Clicks_EventOrganizedId",
                table: "Clicks",
                column: "EventOrganizedId");

            migrationBuilder.CreateIndex(
                name: "IX_Clicks_ParticipantId",
                table: "Clicks",
                column: "ParticipantId");

            migrationBuilder.CreateIndex(
                name: "IX_Applications_EventOrganizedId",
                table: "Applications",
                column: "EventOrganizedId");

            migrationBuilder.CreateIndex(
                name: "IX_Applications_ParticipantId",
                table: "Applications",
                column: "ParticipantId");

            migrationBuilder.CreateIndex(
                name: "IX_Followings_EventOrganizedId",
                table: "Followings",
                column: "EventOrganizedId");

            migrationBuilder.CreateIndex(
                name: "IX_Followings_ParticipantId",
                table: "Followings",
                column: "ParticipantId");

            migrationBuilder.AddForeignKey(
                name: "FK_Clicks_Events_EventOrganizedId",
                table: "Clicks",
                column: "EventOrganizedId",
                principalTable: "Events",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Clicks_Participants_ParticipantId",
                table: "Clicks",
                column: "ParticipantId",
                principalTable: "Participants",
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
                name: "FK_Comments_Participants_ParticipantId",
                table: "Comments",
                column: "ParticipantId",
                principalTable: "Participants",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Clicks_Events_EventOrganizedId",
                table: "Clicks");

            migrationBuilder.DropForeignKey(
                name: "FK_Clicks_Participants_ParticipantId",
                table: "Clicks");

            migrationBuilder.DropForeignKey(
                name: "FK_Comments_Events_EventOrganizedId",
                table: "Comments");

            migrationBuilder.DropForeignKey(
                name: "FK_Comments_Participants_ParticipantId",
                table: "Comments");

            migrationBuilder.DropTable(
                name: "Applications");

            migrationBuilder.DropTable(
                name: "Followings");

            migrationBuilder.DropIndex(
                name: "IX_Comments_ParticipantId",
                table: "Comments");

            migrationBuilder.DropIndex(
                name: "IX_Clicks_EventOrganizedId",
                table: "Clicks");

            migrationBuilder.DropIndex(
                name: "IX_Clicks_ParticipantId",
                table: "Clicks");

            migrationBuilder.DropColumn(
                name: "ParticipantId",
                table: "Comments");

            migrationBuilder.AlterColumn<Guid>(
                name: "EventOrganizedId",
                table: "Comments",
                type: "uniqueidentifier",
                nullable: false,
                oldClrType: typeof(Guid),
                oldNullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "CommenterId",
                table: "Comments",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "ParticipantId",
                table: "Clicks",
                type: "uniqueidentifier",
                nullable: false,
                oldClrType: typeof(Guid),
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "EventOrganizedId",
                table: "Clicks",
                type: "uniqueidentifier",
                nullable: false,
                oldClrType: typeof(Guid),
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "Interactions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ApplicationText = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EventOrganizedId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ParticipantId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserParticipationType = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Interactions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Interactions_Events_EventOrganizedId",
                        column: x => x.EventOrganizedId,
                        principalTable: "Events",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Comments_CommenterId",
                table: "Comments",
                column: "CommenterId");

            migrationBuilder.CreateIndex(
                name: "IX_Interactions_EventOrganizedId",
                table: "Interactions",
                column: "EventOrganizedId");

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_Participants_CommenterId",
                table: "Comments",
                column: "CommenterId",
                principalTable: "Participants",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

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
