using Microsoft.EntityFrameworkCore.Migrations;

namespace Karma.Migrations
{
    public partial class VolunteerCapAndPictureUrl : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "MaxVolunteers",
                table: "Events",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "PhotoPath",
                table: "Events",
                type: "TEXT",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MaxVolunteers",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "PhotoPath",
                table: "Events");
        }
    }
}
