using Microsoft.EntityFrameworkCore.Migrations;

namespace API.Migrations
{
    public partial class feedbackecipientId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "RecipientId",
                table: "Feedbacks",
                type: "INTEGER",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RecipientId",
                table: "Feedbacks");
        }
    }
}
