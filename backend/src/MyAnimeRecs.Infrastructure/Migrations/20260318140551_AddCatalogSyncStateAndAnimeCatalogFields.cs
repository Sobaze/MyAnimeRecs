using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyAnimeRecs.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddCatalogSyncStateAndAnimeCatalogFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "LastImportAtUtc",
                table: "UserProfiles",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CatalogSource",
                table: "Animes",
                type: "TEXT",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Episodes",
                table: "Animes",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsCatalogSeeded",
                table: "Animes",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastAnimeCatalogUpdateUtc",
                table: "Animes",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MediaType",
                table: "Animes",
                type: "TEXT",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Popularity",
                table: "Animes",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Rank",
                table: "Animes",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "Animes",
                type: "TEXT",
                maxLength: 50,
                nullable: true);

            migrationBuilder.CreateTable(
                name: "CatalogSyncStates",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Provider = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    SyncType = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    LastOffset = table.Column<int>(type: "INTEGER", nullable: false),
                    LastRunAtUtc = table.Column<DateTime>(type: "TEXT", nullable: true),
                    LastSuccessAtUtc = table.Column<DateTime>(type: "TEXT", nullable: true),
                    LastError = table.Column<string>(type: "TEXT", maxLength: 2000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CatalogSyncStates", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CatalogSyncStates_Provider_SyncType",
                table: "CatalogSyncStates",
                columns: new[] { "Provider", "SyncType" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CatalogSyncStates");

            migrationBuilder.DropColumn(
                name: "LastImportAtUtc",
                table: "UserProfiles");

            migrationBuilder.DropColumn(
                name: "CatalogSource",
                table: "Animes");

            migrationBuilder.DropColumn(
                name: "Episodes",
                table: "Animes");

            migrationBuilder.DropColumn(
                name: "IsCatalogSeeded",
                table: "Animes");

            migrationBuilder.DropColumn(
                name: "LastAnimeCatalogUpdateUtc",
                table: "Animes");

            migrationBuilder.DropColumn(
                name: "MediaType",
                table: "Animes");

            migrationBuilder.DropColumn(
                name: "Popularity",
                table: "Animes");

            migrationBuilder.DropColumn(
                name: "Rank",
                table: "Animes");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Animes");
        }
    }
}
