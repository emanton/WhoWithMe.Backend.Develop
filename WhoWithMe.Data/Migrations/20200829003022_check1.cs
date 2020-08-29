using Microsoft.EntityFrameworkCore.Migrations;

namespace WhoWithMe.Data.Migrations
{
    public partial class check1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MeetingImage_Meeting_MeetingId",
                table: "MeetingImage");

            migrationBuilder.AlterColumn<long>(
                name: "MeetingId",
                table: "MeetingImage",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_MeetingImage_Meeting_MeetingId",
                table: "MeetingImage",
                column: "MeetingId",
                principalTable: "Meeting",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MeetingImage_Meeting_MeetingId",
                table: "MeetingImage");

            migrationBuilder.AlterColumn<long>(
                name: "MeetingId",
                table: "MeetingImage",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long));

            migrationBuilder.AddForeignKey(
                name: "FK_MeetingImage_Meeting_MeetingId",
                table: "MeetingImage",
                column: "MeetingId",
                principalTable: "Meeting",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
