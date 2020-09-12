using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WhoWithMe.Data.Migrations
{
    public partial class UpdatedMeeting : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "InviterNickname",
                table: "User",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Address",
                table: "Meeting",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "Meeting",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "StartDate",
                table: "Meeting",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "InviterNickname",
                table: "User");

            migrationBuilder.DropColumn(
                name: "Address",
                table: "Meeting");

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "Meeting");

            migrationBuilder.DropColumn(
                name: "StartDate",
                table: "Meeting");
        }
    }
}
