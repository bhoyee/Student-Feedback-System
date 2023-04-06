using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace API.Migrations
{
    public partial class FeedbackEntitiesUpdate1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AppUserId",
                table: "Feedbacks",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsPublic",
                table: "FeedbackReplies",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "FeedbackReplies",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.CreateIndex(
                name: "IX_Feedbacks_AppUserId",
                table: "Feedbacks",
                column: "AppUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Feedbacks_AspNetUsers_AppUserId",
                table: "Feedbacks",
                column: "AppUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Feedbacks_AspNetUsers_AppUserId",
                table: "Feedbacks");

            migrationBuilder.DropIndex(
                name: "IX_Feedbacks_AppUserId",
                table: "Feedbacks");

            migrationBuilder.DropColumn(
                name: "AppUserId",
                table: "Feedbacks");

            migrationBuilder.DropColumn(
                name: "IsPublic",
                table: "FeedbackReplies");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "FeedbackReplies");
        }
    }
}
