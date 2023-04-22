using Microsoft.EntityFrameworkCore.Migrations;

namespace API.Migrations
{
    public partial class updatedFeedbackTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TargetAudience",
                table: "Feedbacks",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TargetAudience",
                table: "Feedbacks");
        }
    }
}
