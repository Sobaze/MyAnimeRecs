using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyAnimeRecs.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddAnimeMainPictureUrls : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "MainPictureLargeUrl",
                table: "Animes",
                type: "TEXT",
                maxLength: 1000,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MainPictureMediumUrl",
                table: "Animes",
                type: "TEXT",
                maxLength: 1000,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MainPictureLargeUrl",
                table: "Animes");

            migrationBuilder.DropColumn(
                name: "MainPictureMediumUrl",
                table: "Animes");
        }
    }
}
