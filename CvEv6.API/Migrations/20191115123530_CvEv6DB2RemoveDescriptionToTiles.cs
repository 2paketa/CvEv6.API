using Microsoft.EntityFrameworkCore.Migrations;

namespace CvEv6.API.Migrations
{
    public partial class CvEv6DB2RemoveDescriptionToTiles : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "Titles");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Titles",
                nullable: true);
        }
    }
}
