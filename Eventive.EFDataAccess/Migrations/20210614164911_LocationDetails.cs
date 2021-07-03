using Microsoft.EntityFrameworkCore.Migrations;

namespace Eventive.EFDataAccess.Migrations
{
    public partial class LocationDetails : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "City",
                table: "EventDetails",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "Latitude",
                table: "EventDetails",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "Longitude",
                table: "EventDetails",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "City",
                table: "EventDetails");

            migrationBuilder.DropColumn(
                name: "Latitude",
                table: "EventDetails");

            migrationBuilder.DropColumn(
                name: "Longitude",
                table: "EventDetails");
        }
    }
}
