using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WhoWithMe.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddCommentText : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Text",
                table: "CommentMeeting",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Text",
                table: "CommentMeeting");
        }
    }
}
