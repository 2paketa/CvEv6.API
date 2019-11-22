using Microsoft.EntityFrameworkCore.Migrations;

namespace CvEv6.API.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DomainId",
                table: "Domains");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DomainId",
                table: "Domains",
                nullable: false,
                defaultValue: 0);
        }
    }
}
