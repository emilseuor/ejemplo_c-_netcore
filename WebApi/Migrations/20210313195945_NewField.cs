using Microsoft.EntityFrameworkCore.Migrations;

namespace WebApi.Migrations
{
    public partial class NewField : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_payment_movie_movieId",
                table: "payment");

            migrationBuilder.RenameColumn(
                name: "status",
                table: "payment",
                newName: "Status");

            migrationBuilder.RenameColumn(
                name: "movieId",
                table: "payment",
                newName: "MovieId");

            migrationBuilder.RenameIndex(
                name: "IX_payment_movieId",
                table: "payment",
                newName: "IX_payment_MovieId");

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "payment",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddForeignKey(
                name: "FK_payment_movie_MovieId",
                table: "payment",
                column: "MovieId",
                principalTable: "movie",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_payment_movie_MovieId",
                table: "payment");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "payment");

            migrationBuilder.RenameColumn(
                name: "Status",
                table: "payment",
                newName: "status");

            migrationBuilder.RenameColumn(
                name: "MovieId",
                table: "payment",
                newName: "movieId");

            migrationBuilder.RenameIndex(
                name: "IX_payment_MovieId",
                table: "payment",
                newName: "IX_payment_movieId");

            migrationBuilder.AddForeignKey(
                name: "FK_payment_movie_movieId",
                table: "payment",
                column: "movieId",
                principalTable: "movie",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
