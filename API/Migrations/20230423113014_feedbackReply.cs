using Microsoft.EntityFrameworkCore.Migrations;

namespace API.Migrations
{
    public partial class feedbackReply : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "FeedbackReplies",
                type: "INTEGER",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "FeedbackReplies");
        }
    }
}
