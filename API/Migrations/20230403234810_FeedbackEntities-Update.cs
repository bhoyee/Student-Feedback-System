using Microsoft.EntityFrameworkCore.Migrations;

namespace API.Migrations
{
    public partial class FeedbackEntitiesUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Feedback_AspNetUsers_AssignedToId",
                table: "Feedback");

            migrationBuilder.DropForeignKey(
                name: "FK_Feedback_AspNetUsers_SenderId",
                table: "Feedback");

            migrationBuilder.DropForeignKey(
                name: "FK_Feedback_Departments_DepartmentId",
                table: "Feedback");

            migrationBuilder.DropForeignKey(
                name: "FK_FeedbackReply_AspNetUsers_UserId",
                table: "FeedbackReply");

            migrationBuilder.DropForeignKey(
                name: "FK_FeedbackReply_Feedback_FeedbackId",
                table: "FeedbackReply");

            migrationBuilder.DropPrimaryKey(
                name: "PK_FeedbackReply",
                table: "FeedbackReply");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Feedback",
                table: "Feedback");

            migrationBuilder.RenameTable(
                name: "FeedbackReply",
                newName: "FeedbackReplies");

            migrationBuilder.RenameTable(
                name: "Feedback",
                newName: "Feedbacks");

            migrationBuilder.RenameIndex(
                name: "IX_FeedbackReply_UserId",
                table: "FeedbackReplies",
                newName: "IX_FeedbackReplies_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_FeedbackReply_FeedbackId",
                table: "FeedbackReplies",
                newName: "IX_FeedbackReplies_FeedbackId");

            migrationBuilder.RenameIndex(
                name: "IX_Feedback_SenderId",
                table: "Feedbacks",
                newName: "IX_Feedbacks_SenderId");

            migrationBuilder.RenameIndex(
                name: "IX_Feedback_DepartmentId",
                table: "Feedbacks",
                newName: "IX_Feedbacks_DepartmentId");

            migrationBuilder.RenameIndex(
                name: "IX_Feedback_AssignedToId",
                table: "Feedbacks",
                newName: "IX_Feedbacks_AssignedToId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_FeedbackReplies",
                table: "FeedbackReplies",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Feedbacks",
                table: "Feedbacks",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_FeedbackReplies_AspNetUsers_UserId",
                table: "FeedbackReplies",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_FeedbackReplies_Feedbacks_FeedbackId",
                table: "FeedbackReplies",
                column: "FeedbackId",
                principalTable: "Feedbacks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Feedbacks_AspNetUsers_AssignedToId",
                table: "Feedbacks",
                column: "AssignedToId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Feedbacks_AspNetUsers_SenderId",
                table: "Feedbacks",
                column: "SenderId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Feedbacks_Departments_DepartmentId",
                table: "Feedbacks",
                column: "DepartmentId",
                principalTable: "Departments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FeedbackReplies_AspNetUsers_UserId",
                table: "FeedbackReplies");

            migrationBuilder.DropForeignKey(
                name: "FK_FeedbackReplies_Feedbacks_FeedbackId",
                table: "FeedbackReplies");

            migrationBuilder.DropForeignKey(
                name: "FK_Feedbacks_AspNetUsers_AssignedToId",
                table: "Feedbacks");

            migrationBuilder.DropForeignKey(
                name: "FK_Feedbacks_AspNetUsers_SenderId",
                table: "Feedbacks");

            migrationBuilder.DropForeignKey(
                name: "FK_Feedbacks_Departments_DepartmentId",
                table: "Feedbacks");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Feedbacks",
                table: "Feedbacks");

            migrationBuilder.DropPrimaryKey(
                name: "PK_FeedbackReplies",
                table: "FeedbackReplies");

            migrationBuilder.RenameTable(
                name: "Feedbacks",
                newName: "Feedback");

            migrationBuilder.RenameTable(
                name: "FeedbackReplies",
                newName: "FeedbackReply");

            migrationBuilder.RenameIndex(
                name: "IX_Feedbacks_SenderId",
                table: "Feedback",
                newName: "IX_Feedback_SenderId");

            migrationBuilder.RenameIndex(
                name: "IX_Feedbacks_DepartmentId",
                table: "Feedback",
                newName: "IX_Feedback_DepartmentId");

            migrationBuilder.RenameIndex(
                name: "IX_Feedbacks_AssignedToId",
                table: "Feedback",
                newName: "IX_Feedback_AssignedToId");

            migrationBuilder.RenameIndex(
                name: "IX_FeedbackReplies_UserId",
                table: "FeedbackReply",
                newName: "IX_FeedbackReply_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_FeedbackReplies_FeedbackId",
                table: "FeedbackReply",
                newName: "IX_FeedbackReply_FeedbackId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Feedback",
                table: "Feedback",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_FeedbackReply",
                table: "FeedbackReply",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Feedback_AspNetUsers_AssignedToId",
                table: "Feedback",
                column: "AssignedToId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Feedback_AspNetUsers_SenderId",
                table: "Feedback",
                column: "SenderId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Feedback_Departments_DepartmentId",
                table: "Feedback",
                column: "DepartmentId",
                principalTable: "Departments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_FeedbackReply_AspNetUsers_UserId",
                table: "FeedbackReply",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_FeedbackReply_Feedback_FeedbackId",
                table: "FeedbackReply",
                column: "FeedbackId",
                principalTable: "Feedback",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
