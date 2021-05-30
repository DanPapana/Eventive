using Microsoft.EntityFrameworkCore.Migrations;

namespace Eventive.EFDataAccess.Migrations
{
    public partial class RatingList : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Ratings_ParticipantId",
                table: "Ratings",
                column: "ParticipantId");

            migrationBuilder.AddForeignKey(
                name: "FK_Ratings_Users_ParticipantId",
                table: "Ratings",
                column: "ParticipantId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Ratings_Users_ParticipantId",
                table: "Ratings");

            migrationBuilder.DropIndex(
                name: "IX_Ratings_ParticipantId",
                table: "Ratings");
        }
    }
}
