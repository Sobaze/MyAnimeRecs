using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyAnimeRecs.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddGenreAndUserAnimeEntries : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Recommendations_Animes_AnimeId",
                table: "Recommendations");

            migrationBuilder.DropForeignKey(
                name: "FK_Recommendations_UserProfiles_UserProfileId",
                table: "Recommendations");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserProfiles",
                table: "UserProfiles");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Recommendations",
                table: "Recommendations");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Animes",
                table: "Animes");

            migrationBuilder.DropIndex(
                name: "IX_Animes_Source_SourceAnimeId",
                table: "Animes");

            migrationBuilder.DropColumn(
                name: "Source",
                table: "Animes");

            migrationBuilder.RenameTable(
                name: "UserProfiles",
                newName: "UserProfilesSet");

            migrationBuilder.RenameTable(
                name: "Recommendations",
                newName: "RecommendationsSet");

            migrationBuilder.RenameTable(
                name: "Animes",
                newName: "AnimesSet");

            migrationBuilder.RenameIndex(
                name: "IX_UserProfiles_Username",
                table: "UserProfilesSet",
                newName: "IX_UserProfilesSet_Username");

            migrationBuilder.RenameIndex(
                name: "IX_Recommendations_UserProfileId_AnimeId",
                table: "RecommendationsSet",
                newName: "IX_RecommendationsSet_UserProfileId_AnimeId");

            migrationBuilder.RenameIndex(
                name: "IX_Recommendations_AnimeId",
                table: "RecommendationsSet",
                newName: "IX_RecommendationsSet_AnimeId");

            migrationBuilder.AddColumn<string>(
                name: "AlgorithmVersion",
                table: "RecommendationsSet",
                type: "TEXT",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<decimal>(
                name: "MeanScore",
                table: "AnimesSet",
                type: "TEXT",
                precision: 4,
                scale: 2,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SourceType",
                table: "AnimesSet",
                type: "TEXT",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserProfilesSet",
                table: "UserProfilesSet",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_RecommendationsSet",
                table: "RecommendationsSet",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AnimesSet",
                table: "AnimesSet",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "GenresSet",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GenresSet", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserAnimeEntriesSet",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    UserProfileId = table.Column<Guid>(type: "TEXT", nullable: false),
                    AnimeId = table.Column<Guid>(type: "TEXT", nullable: false),
                    Status = table.Column<string>(type: "TEXT", maxLength: 30, nullable: false),
                    Score = table.Column<decimal>(type: "TEXT", precision: 4, scale: 2, nullable: true),
                    SourceType = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    SourceUserName = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    CreatedAtUtc = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdatedAtUtc = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserAnimeEntriesSet", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserAnimeEntriesSet_AnimesSet_AnimeId",
                        column: x => x.AnimeId,
                        principalTable: "AnimesSet",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserAnimeEntriesSet_UserProfilesSet_UserProfileId",
                        column: x => x.UserProfileId,
                        principalTable: "UserProfilesSet",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AnimeGenresSet",
                columns: table => new
                {
                    AnimeId = table.Column<Guid>(type: "TEXT", nullable: false),
                    GenreId = table.Column<Guid>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AnimeGenresSet", x => new { x.AnimeId, x.GenreId });
                    table.ForeignKey(
                        name: "FK_AnimeGenresSet_AnimesSet_AnimeId",
                        column: x => x.AnimeId,
                        principalTable: "AnimesSet",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AnimeGenresSet_GenresSet_GenreId",
                        column: x => x.GenreId,
                        principalTable: "GenresSet",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AnimesSet_SourceType_SourceAnimeId",
                table: "AnimesSet",
                columns: new[] { "SourceType", "SourceAnimeId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AnimeGenresSet_GenreId",
                table: "AnimeGenresSet",
                column: "GenreId");

            migrationBuilder.CreateIndex(
                name: "IX_GenresSet_Name",
                table: "GenresSet",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserAnimeEntriesSet_AnimeId",
                table: "UserAnimeEntriesSet",
                column: "AnimeId");

            migrationBuilder.CreateIndex(
                name: "IX_UserAnimeEntriesSet_UserProfileId_AnimeId",
                table: "UserAnimeEntriesSet",
                columns: new[] { "UserProfileId", "AnimeId" },
                unique: true);

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
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RecommendationsSet_AnimesSet_AnimeId",
                table: "RecommendationsSet");

            migrationBuilder.DropForeignKey(
                name: "FK_RecommendationsSet_UserProfilesSet_UserProfileId",
                table: "RecommendationsSet");

            migrationBuilder.DropTable(
                name: "AnimeGenresSet");

            migrationBuilder.DropTable(
                name: "UserAnimeEntriesSet");

            migrationBuilder.DropTable(
                name: "GenresSet");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserProfilesSet",
                table: "UserProfilesSet");

            migrationBuilder.DropPrimaryKey(
                name: "PK_RecommendationsSet",
                table: "RecommendationsSet");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AnimesSet",
                table: "AnimesSet");

            migrationBuilder.DropIndex(
                name: "IX_AnimesSet_SourceType_SourceAnimeId",
                table: "AnimesSet");

            migrationBuilder.DropColumn(
                name: "AlgorithmVersion",
                table: "RecommendationsSet");

            migrationBuilder.DropColumn(
                name: "MeanScore",
                table: "AnimesSet");

            migrationBuilder.DropColumn(
                name: "SourceType",
                table: "AnimesSet");

            migrationBuilder.RenameTable(
                name: "UserProfilesSet",
                newName: "UserProfiles");

            migrationBuilder.RenameTable(
                name: "RecommendationsSet",
                newName: "Recommendations");

            migrationBuilder.RenameTable(
                name: "AnimesSet",
                newName: "Animes");

            migrationBuilder.RenameIndex(
                name: "IX_UserProfilesSet_Username",
                table: "UserProfiles",
                newName: "IX_UserProfiles_Username");

            migrationBuilder.RenameIndex(
                name: "IX_RecommendationsSet_UserProfileId_AnimeId",
                table: "Recommendations",
                newName: "IX_Recommendations_UserProfileId_AnimeId");

            migrationBuilder.RenameIndex(
                name: "IX_RecommendationsSet_AnimeId",
                table: "Recommendations",
                newName: "IX_Recommendations_AnimeId");

            migrationBuilder.AddColumn<string>(
                name: "Source",
                table: "Animes",
                type: "TEXT",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserProfiles",
                table: "UserProfiles",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Recommendations",
                table: "Recommendations",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Animes",
                table: "Animes",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Animes_Source_SourceAnimeId",
                table: "Animes",
                columns: new[] { "Source", "SourceAnimeId" },
                unique: true);

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
        }
    }
}
