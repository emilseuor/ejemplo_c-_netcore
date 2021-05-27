using Microsoft.EntityFrameworkCore.Migrations;

namespace WebApi.Migrations
{
    public partial class Changes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "MovieId",
                table: "likes",
                newName: "movieId");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "likes",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateIndex(
                name: "IX_likes_movieId",
                table: "likes",
                column: "movieId");

            migrationBuilder.CreateIndex(
                name: "IX_likes_UserId",
                table: "likes",
                column: "UserId");

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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_likes_AspNetUsers_UserId",
                table: "likes");

            migrationBuilder.DropForeignKey(
                name: "FK_likes_movie_movieId",
                table: "likes");

            migrationBuilder.DropIndex(
                name: "IX_likes_movieId",
                table: "likes");

            migrationBuilder.DropIndex(
                name: "IX_likes_UserId",
                table: "likes");

            migrationBuilder.RenameColumn(
                name: "movieId",
                table: "likes",
                newName: "MovieId");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "likes",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);
        }
    }
}
