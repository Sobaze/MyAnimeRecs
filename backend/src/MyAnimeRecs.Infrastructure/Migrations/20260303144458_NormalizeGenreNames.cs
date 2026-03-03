using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyAnimeRecs.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class NormalizeGenreNames : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Genres_Name",
                table: "Genres");

            migrationBuilder.AddColumn<string>(
                name: "NormalizedName",
                table: "Genres",
                type: "TEXT",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.Sql(@"
UPDATE ""Genres""
SET ""NormalizedName"" = lower(trim(""Name""));

CREATE TEMP TABLE ""_genre_merge"" AS
SELECT
    ""Id"" AS ""DuplicateId"",
    min(""Id"") OVER (PARTITION BY ""NormalizedName"") AS ""KeepId""
FROM ""Genres"";

UPDATE ""AnimeGenres""
SET ""GenreId"" = (
    SELECT ""KeepId""
    FROM ""_genre_merge""
    WHERE ""DuplicateId"" = ""AnimeGenres"".""GenreId""
)
WHERE EXISTS (
    SELECT 1
    FROM ""_genre_merge""
    WHERE ""DuplicateId"" = ""AnimeGenres"".""GenreId""
      AND ""DuplicateId"" <> ""KeepId""
);

DELETE FROM ""AnimeGenres""
WHERE rowid NOT IN (
    SELECT min(rowid)
    FROM ""AnimeGenres""
    GROUP BY ""AnimeId"", ""GenreId""
);

DELETE FROM ""Genres""
WHERE ""Id"" IN (
    SELECT ""DuplicateId""
    FROM ""_genre_merge""
    WHERE ""DuplicateId"" <> ""KeepId""
);

DROP TABLE ""_genre_merge"";
");

            migrationBuilder.CreateIndex(
                name: "IX_Genres_NormalizedName",
                table: "Genres",
                column: "NormalizedName",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Genres_NormalizedName",
                table: "Genres");

            migrationBuilder.DropColumn(
                name: "NormalizedName",
                table: "Genres");

            migrationBuilder.CreateIndex(
                name: "IX_Genres_Name",
                table: "Genres",
                column: "Name",
                unique: true);
        }
    }
}
