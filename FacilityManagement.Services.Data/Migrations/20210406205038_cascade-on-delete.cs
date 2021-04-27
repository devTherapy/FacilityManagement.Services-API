using Microsoft.EntityFrameworkCore.Migrations;

namespace FacilityManagement.Services.Data.Migrations
{
    public partial class cascadeondelete : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Replies_Comments_CommentsId",
                table: "Replies");

            migrationBuilder.DropIndex(
                name: "IX_Replies_CommentsId",
                table: "Replies");

            migrationBuilder.DropColumn(
                name: "CommentsId",
                table: "Replies");

            migrationBuilder.AddColumn<string>(
                name: "CommentId",
                table: "Replies",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Replies_CommentId",
                table: "Replies",
                column: "CommentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Replies_Comments_CommentId",
                table: "Replies",
                column: "CommentId",
                principalTable: "Comments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Replies_Comments_CommentId",
                table: "Replies");

            migrationBuilder.DropIndex(
                name: "IX_Replies_CommentId",
                table: "Replies");

            migrationBuilder.DropColumn(
                name: "CommentId",
                table: "Replies");

            migrationBuilder.AddColumn<string>(
                name: "CommentsId",
                table: "Replies",
                type: "text",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Replies_CommentsId",
                table: "Replies",
                column: "CommentsId");

            migrationBuilder.AddForeignKey(
                name: "FK_Replies_Comments_CommentsId",
                table: "Replies",
                column: "CommentsId",
                principalTable: "Comments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
