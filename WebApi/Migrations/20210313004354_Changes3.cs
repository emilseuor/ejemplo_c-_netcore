using Microsoft.EntityFrameworkCore.Migrations;

namespace WebApi.Migrations
{
    public partial class Changes3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_like_movie_movieId",
                table: "like");

            migrationBuilder.RenameColumn(
                name: "movieId",
                table: "like",
                newName: "MovieId");

            migrationBuilder.RenameIndex(
                name: "IX_like_movieId",
                table: "like",
                newName: "IX_like_MovieId");

            migrationBuilder.AddForeignKey(
                name: "FK_like_movie_MovieId",
                table: "like",
                column: "MovieId",
                principalTable: "movie",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_like_movie_MovieId",
                table: "like");

            migrationBuilder.RenameColumn(
                name: "MovieId",
                table: "like",
                newName: "movieId");

            migrationBuilder.RenameIndex(
                name: "IX_like_MovieId",
                table: "like",
                newName: "IX_like_movieId");

            migrationBuilder.AddForeignKey(
                name: "FK_like_movie_movieId",
                table: "like",
                column: "movieId",
                principalTable: "movie",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
