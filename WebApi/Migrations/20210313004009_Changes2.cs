using Microsoft.EntityFrameworkCore.Migrations;

namespace WebApi.Migrations
{
    public partial class Changes2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_likes_AspNetUsers_UserId",
                table: "likes");

            migrationBuilder.DropForeignKey(
                name: "FK_likes_movie_movieId",
                table: "likes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_likes",
                table: "likes");

            migrationBuilder.RenameTable(
                name: "likes",
                newName: "like");

            migrationBuilder.RenameIndex(
                name: "IX_likes_UserId",
                table: "like",
                newName: "IX_like_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_likes_movieId",
                table: "like",
                newName: "IX_like_movieId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_like",
                table: "like",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_like_AspNetUsers_UserId",
                table: "like",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_like_movie_movieId",
                table: "like",
                column: "movieId",
                principalTable: "movie",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_like_AspNetUsers_UserId",
                table: "like");

            migrationBuilder.DropForeignKey(
                name: "FK_like_movie_movieId",
                table: "like");

            migrationBuilder.DropPrimaryKey(
                name: "PK_like",
                table: "like");

            migrationBuilder.RenameTable(
                name: "like",
                newName: "likes");

            migrationBuilder.RenameIndex(
                name: "IX_like_UserId",
                table: "likes",
                newName: "IX_likes_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_like_movieId",
                table: "likes",
                newName: "IX_likes_movieId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_likes",
                table: "likes",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_likes_AspNetUsers_UserId",
                table: "likes",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_likes_movie_movieId",
                table: "likes",
                column: "movieId",
                principalTable: "movie",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
