using Microsoft.EntityFrameworkCore.Migrations;

namespace Eventive.EFDataAccess.Migrations
{
    public partial class Renaming : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comments_Users_CommenterId",
                table: "Comments");

            migrationBuilder.DropForeignKey(
                name: "FK_Ratings_Users_ParticipantId",
                table: "Ratings");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_ContactDetails_ContactDetailsId",
                table: "Users");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Users",
                table: "Users");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Participations",
                table: "Interaction");

            migrationBuilder.RenameTable(
                name: "Users",
                newName: "Participants");

            migrationBuilder.RenameTable(
                name: "Interaction",
                newName: "Interactions");

            migrationBuilder.RenameIndex(
                name: "IX_Users_ContactDetailsId",
                table: "Participants",
                newName: "IX_Participants_ContactDetailsId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Participants",
                table: "Participants",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Interactions",
                table: "Interactions",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_Participants_CommenterId",
                table: "Comments",
                column: "CommenterId",
                principalTable: "Participants",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Participants_ContactDetails_ContactDetailsId",
                table: "Participants",
                column: "ContactDetailsId",
                principalTable: "ContactDetails",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Ratings_Participants_ParticipantId",
                table: "Ratings",
                column: "ParticipantId",
                principalTable: "Participants",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comments_Participants_CommenterId",
                table: "Comments");

            migrationBuilder.DropForeignKey(
                name: "FK_Participants_ContactDetails_ContactDetailsId",
                table: "Participants");

            migrationBuilder.DropForeignKey(
                name: "FK_Ratings_Participants_ParticipantId",
                table: "Ratings");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Participants",
                table: "Participants");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Interactions",
                table: "Interactions");

            migrationBuilder.RenameTable(
                name: "Participants",
                newName: "Users");

            migrationBuilder.RenameTable(
                name: "Interactions",
                newName: "Interaction");

            migrationBuilder.RenameIndex(
                name: "IX_Participants_ContactDetailsId",
                table: "Users",
                newName: "IX_Users_ContactDetailsId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Users",
                table: "Users",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Participations",
                table: "Interaction",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_Users_CommenterId",
                table: "Comments",
                column: "CommenterId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Ratings_Users_ParticipantId",
                table: "Ratings",
                column: "ParticipantId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Users_ContactDetails_ContactDetailsId",
                table: "Users",
                column: "ContactDetailsId",
                principalTable: "ContactDetails",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
