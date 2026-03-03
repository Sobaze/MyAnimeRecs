using Microsoft.EntityFrameworkCore;
using MyAnimeRecs.Application.Abstractions;
using MyAnimeRecs.Domain.Entities;

namespace MyAnimeRecs.Infrastructure.Persistence
{
    public class MyAnimeDBContext : DbContext, IApplicationDbContext
    {
        public MyAnimeDBContext(DbContextOptions<MyAnimeDBContext> options) : base(options)
        {
        }

        public DbSet<Anime> Animes => Set<Anime>();
        public DbSet<UserProfile> UserProfiles => Set<UserProfile>();
        public DbSet<Genre> Genres => Set<Genre>();
        public DbSet<AnimeGenre> AnimeGenres => Set<AnimeGenre>();
        public DbSet<UserAnimeEntry> UserAnimeEntries => Set<UserAnimeEntry>();
        public DbSet<Recommendation> Recommendations => Set<Recommendation>();

        IQueryable<Anime> IApplicationDbContext.Animes => Animes;
        IQueryable<UserProfile> IApplicationDbContext.UserProfiles => UserProfiles;
        IQueryable<Genre> IApplicationDbContext.Genres => Genres;
        IQueryable<AnimeGenre> IApplicationDbContext.AnimeGenres => AnimeGenres;
        IQueryable<UserAnimeEntry> IApplicationDbContext.UserAnimeEntries => UserAnimeEntries;
        IQueryable<Recommendation> IApplicationDbContext.Recommendations => Recommendations;

        void IApplicationDbContext.Add<T>(T entity) => base.Add(entity);

        void IApplicationDbContext.AddRange<T>(IEnumerable<T> entities) => base.AddRange(entities);

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Anime>(entity =>
            {
                entity.HasKey(x => x.Id);
                entity.Property(x => x.Title).IsRequired().HasMaxLength(250);
                entity.Property(x => x.SourceType).HasConversion<string>().IsRequired().HasMaxLength(50);
                entity.Property(x => x.SourceAnimeId).IsRequired().HasMaxLength(100);
                entity.Property(x => x.MeanScore).HasPrecision(4, 2);
                entity.Property(x => x.MainPictureMediumUrl).HasMaxLength(1000);
                entity.Property(x => x.MainPictureLargeUrl).HasMaxLength(1000);
                entity.Property(x => x.CreatedAtUtc).IsRequired();
                entity.HasIndex(x => new { x.SourceType, x.SourceAnimeId }).IsUnique();
            });

            modelBuilder.Entity<UserProfile>(entity =>
            {
                entity.HasKey(x => x.Id);
                entity.Property(x => x.Username).IsRequired().HasMaxLength(100);
                entity.Property(x => x.CreatedAtUtc).IsRequired();
                entity.HasIndex(x => x.Username).IsUnique();
            });

            modelBuilder.Entity<Recommendation>(entity =>
            {
                entity.HasKey(x => x.Id);
                entity.Property(x => x.Score).HasPrecision(4, 2);
                entity.Property(x => x.Reason).HasMaxLength(500);
                entity.Property(x => x.AlgorithmVersion).IsRequired().HasMaxLength(50);
                entity.Property(x => x.CreatedAtUtc).IsRequired();

                entity.HasOne(x => x.UserProfile)
                    .WithMany(x => x.Recommendations)
                    .HasForeignKey(x => x.UserProfileId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(x => x.Anime)
                    .WithMany(x => x.Recommendations)
                    .HasForeignKey(x => x.AnimeId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasIndex(x => new { x.UserProfileId, x.AnimeId }).IsUnique();
            });

            modelBuilder.Entity<Genre>(entity =>
            {
                entity.HasKey(x => x.Id);
                entity.Property(x => x.Name).IsRequired().HasMaxLength(100);
                entity.Property(x => x.NormalizedName).IsRequired().HasMaxLength(100);
                entity.HasIndex(x => x.NormalizedName).IsUnique();
            });

            modelBuilder.Entity<AnimeGenre>(entity =>
            {
                entity.HasKey(x => new { x.AnimeId, x.GenreId });

                entity.HasOne(x => x.Anime)
                    .WithMany(x => x.AnimeGenres)
                    .HasForeignKey(x => x.AnimeId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(x => x.Genre)
                    .WithMany(x => x.AnimeGenres)
                    .HasForeignKey(x => x.GenreId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<UserAnimeEntry>(entity =>
            {
                entity.HasKey(x => x.Id);
                entity.Property(x => x.Status).HasConversion<string>().IsRequired().HasMaxLength(30);
                entity.Property(x => x.Score).HasPrecision(4, 2);
                entity.Property(x => x.SourceType).HasConversion<string>().IsRequired().HasMaxLength(50);
                entity.Property(x => x.SourceUserName).IsRequired().HasMaxLength(100);
                entity.Property(x => x.CreatedAtUtc).IsRequired();
                entity.Property(x => x.UpdatedAtUtc).IsRequired();

                entity.HasOne(x => x.UserProfile)
                    .WithMany(x => x.UserAnimeEntries)
                    .HasForeignKey(x => x.UserProfileId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(x => x.Anime)
                    .WithMany(x => x.UserAnimeEntries)
                    .HasForeignKey(x => x.AnimeId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasIndex(x => new { x.UserProfileId, x.AnimeId }).IsUnique();
            });
        }
    }
}
