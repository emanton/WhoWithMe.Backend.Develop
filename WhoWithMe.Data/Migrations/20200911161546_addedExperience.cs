using Microsoft.EntityFrameworkCore.Migrations;

namespace WhoWithMe.Data.Migrations
{
    public partial class addedExperience : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "Experience",
                table: "User",
                nullable: false,
                defaultValue: 0L);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Experience",
                table: "User");
        }
    }
}
