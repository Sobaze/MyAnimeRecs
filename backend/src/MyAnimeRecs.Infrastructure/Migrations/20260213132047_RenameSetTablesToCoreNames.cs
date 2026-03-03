using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyAnimeRecs.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RenameSetTablesToCoreNames : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AnimeGenresSet_AnimesSet_AnimeId",
                table: "AnimeGenresSet");

            migrationBuilder.DropForeignKey(
                name: "FK_AnimeGenresSet_GenresSet_GenreId",
                table: "AnimeGenresSet");

            migrationBuilder.DropForeignKey(
                name: "FK_RecommendationsSet_AnimesSet_AnimeId",
                table: "RecommendationsSet");

            migrationBuilder.DropForeignKey(
                name: "FK_RecommendationsSet_UserProfilesSet_UserProfileId",
                table: "RecommendationsSet");

            migrationBuilder.DropForeignKey(
                name: "FK_UserAnimeEntriesSet_AnimesSet_AnimeId",
                table: "UserAnimeEntriesSet");

            migrationBuilder.DropForeignKey(
                name: "FK_UserAnimeEntriesSet_UserProfilesSet_UserProfileId",
                table: "UserAnimeEntriesSet");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserProfilesSet",
                table: "UserProfilesSet");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserAnimeEntriesSet",
                table: "UserAnimeEntriesSet");

            migrationBuilder.DropPrimaryKey(
                name: "PK_RecommendationsSet",
                table: "RecommendationsSet");

            migrationBuilder.DropPrimaryKey(
                name: "PK_GenresSet",
                table: "GenresSet");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AnimesSet",
                table: "AnimesSet");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AnimeGenresSet",
                table: "AnimeGenresSet");

            migrationBuilder.RenameTable(
                name: "UserProfilesSet",
                newName: "UserProfiles");

            migrationBuilder.RenameTable(
                name: "UserAnimeEntriesSet",
                newName: "UserAnimeEntries");

            migrationBuilder.RenameTable(
                name: "RecommendationsSet",
                newName: "Recommendations");

            migrationBuilder.RenameTable(
                name: "GenresSet",
                newName: "Genres");

            migrationBuilder.RenameTable(
                name: "AnimesSet",
                newName: "Animes");

            migrationBuilder.RenameTable(
                name: "AnimeGenresSet",
                newName: "AnimeGenres");

            migrationBuilder.RenameIndex(
                name: "IX_UserProfilesSet_Username",
                table: "UserProfiles",
                newName: "IX_UserProfiles_Username");

            migrationBuilder.RenameIndex(
                name: "IX_UserAnimeEntriesSet_UserProfileId_AnimeId",
                table: "UserAnimeEntries",
                newName: "IX_UserAnimeEntries_UserProfileId_AnimeId");

            migrationBuilder.RenameIndex(
                name: "IX_UserAnimeEntriesSet_AnimeId",
                table: "UserAnimeEntries",
                newName: "IX_UserAnimeEntries_AnimeId");

            migrationBuilder.RenameIndex(
                name: "IX_RecommendationsSet_UserProfileId_AnimeId",
                table: "Recommendations",
                newName: "IX_Recommendations_UserProfileId_AnimeId");

            migrationBuilder.RenameIndex(
                name: "IX_RecommendationsSet_AnimeId",
                table: "Recommendations",
                newName: "IX_Recommendations_AnimeId");

            migrationBuilder.RenameIndex(
                name: "IX_GenresSet_Name",
                table: "Genres",
                newName: "IX_Genres_Name");

            migrationBuilder.RenameIndex(
                name: "IX_AnimesSet_SourceType_SourceAnimeId",
                table: "Animes",
                newName: "IX_Animes_SourceType_SourceAnimeId");

            migrationBuilder.RenameIndex(
                name: "IX_AnimeGenresSet_GenreId",
                table: "AnimeGenres",
                newName: "IX_AnimeGenres_GenreId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserProfiles",
                table: "UserProfiles",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserAnimeEntries",
                table: "UserAnimeEntries",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Recommendations",
                table: "Recommendations",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Genres",
                table: "Genres",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Animes",
                table: "Animes",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AnimeGenres",
                table: "AnimeGenres",
                columns: new[] { "AnimeId", "GenreId" });

            migrationBuilder.AddForeignKey(
                name: "FK_AnimeGenres_Animes_AnimeId",
                table: "AnimeGenres",
                column: "AnimeId",
                principalTable: "Animes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AnimeGenres_Genres_GenreId",
                table: "AnimeGenres",
                column: "GenreId",
                principalTable: "Genres",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Recommendations_Animes_AnimeId",
                table: "Recommendations",
                column: "AnimeId",
                principalTable: "Animes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Recommendations_UserProfiles_UserProfileId",
                table: "Recommendations",
                column: "UserProfileId",
                principalTable: "UserProfiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserAnimeEntries_Animes_AnimeId",
                table: "UserAnimeEntries",
                column: "AnimeId",
                principalTable: "Animes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserAnimeEntries_UserProfiles_UserProfileId",
                table: "UserAnimeEntries",
                column: "UserProfileId",
                principalTable: "UserProfiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AnimeGenres_Animes_AnimeId",
                table: "AnimeGenres");

            migrationBuilder.DropForeignKey(
                name: "FK_AnimeGenres_Genres_GenreId",
                table: "AnimeGenres");

            migrationBuilder.DropForeignKey(
                name: "FK_Recommendations_Animes_AnimeId",
                table: "Recommendations");

            migrationBuilder.DropForeignKey(
                name: "FK_Recommendations_UserProfiles_UserProfileId",
                table: "Recommendations");

            migrationBuilder.DropForeignKey(
                name: "FK_UserAnimeEntries_Animes_AnimeId",
                table: "UserAnimeEntries");

            migrationBuilder.DropForeignKey(
                name: "FK_UserAnimeEntries_UserProfiles_UserProfileId",
                table: "UserAnimeEntries");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserProfiles",
                table: "UserProfiles");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserAnimeEntries",
                table: "UserAnimeEntries");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Recommendations",
                table: "Recommendations");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Genres",
                table: "Genres");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Animes",
                table: "Animes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AnimeGenres",
                table: "AnimeGenres");

            migrationBuilder.RenameTable(
                name: "UserProfiles",
                newName: "UserProfilesSet");

            migrationBuilder.RenameTable(
                name: "UserAnimeEntries",
                newName: "UserAnimeEntriesSet");

            migrationBuilder.RenameTable(
                name: "Recommendations",
                newName: "RecommendationsSet");

            migrationBuilder.RenameTable(
                name: "Genres",
                newName: "GenresSet");

            migrationBuilder.RenameTable(
                name: "Animes",
                newName: "AnimesSet");

            migrationBuilder.RenameTable(
                name: "AnimeGenres",
                newName: "AnimeGenresSet");

            migrationBuilder.RenameIndex(
                name: "IX_UserProfiles_Username",
                table: "UserProfilesSet",
                newName: "IX_UserProfilesSet_Username");

            migrationBuilder.RenameIndex(
                name: "IX_UserAnimeEntries_UserProfileId_AnimeId",
                table: "UserAnimeEntriesSet",
                newName: "IX_UserAnimeEntriesSet_UserProfileId_AnimeId");

            migrationBuilder.RenameIndex(
                name: "IX_UserAnimeEntries_AnimeId",
                table: "UserAnimeEntriesSet",
                newName: "IX_UserAnimeEntriesSet_AnimeId");

            migrationBuilder.RenameIndex(
                name: "IX_Recommendations_UserProfileId_AnimeId",
                table: "RecommendationsSet",
                newName: "IX_RecommendationsSet_UserProfileId_AnimeId");

            migrationBuilder.RenameIndex(
                name: "IX_Recommendations_AnimeId",
                table: "RecommendationsSet",
                newName: "IX_RecommendationsSet_AnimeId");

            migrationBuilder.RenameIndex(
                name: "IX_Genres_Name",
                table: "GenresSet",
                newName: "IX_GenresSet_Name");

            migrationBuilder.RenameIndex(
                name: "IX_Animes_SourceType_SourceAnimeId",
                table: "AnimesSet",
                newName: "IX_AnimesSet_SourceType_SourceAnimeId");

            migrationBuilder.RenameIndex(
                name: "IX_AnimeGenres_GenreId",
                table: "AnimeGenresSet",
                newName: "IX_AnimeGenresSet_GenreId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserProfilesSet",
                table: "UserProfilesSet",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserAnimeEntriesSet",
                table: "UserAnimeEntriesSet",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_RecommendationsSet",
                table: "RecommendationsSet",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_GenresSet",
                table: "GenresSet",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AnimesSet",
                table: "AnimesSet",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AnimeGenresSet",
                table: "AnimeGenresSet",
                columns: new[] { "AnimeId", "GenreId" });

            migrationBuilder.AddForeignKey(
                name: "FK_AnimeGenresSet_AnimesSet_AnimeId",
                table: "AnimeGenresSet",
                column: "AnimeId",
                principalTable: "AnimesSet",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AnimeGenresSet_GenresSet_GenreId",
                table: "AnimeGenresSet",
                column: "GenreId",
                principalTable: "GenresSet",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RecommendationsSet_AnimesSet_AnimeId",
                table: "RecommendationsSet",
                column: "AnimeId",
                principalTable: "AnimesSet",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RecommendationsSet_UserProfilesSet_UserProfileId",
                table: "RecommendationsSet",
                column: "UserProfileId",
                principalTable: "UserProfilesSet",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserAnimeEntriesSet_AnimesSet_AnimeId",
                table: "UserAnimeEntriesSet",
                column: "AnimeId",
                principalTable: "AnimesSet",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserAnimeEntriesSet_UserProfilesSet_UserProfileId",
                table: "UserAnimeEntriesSet",
                column: "UserProfileId",
                principalTable: "UserProfilesSet",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
