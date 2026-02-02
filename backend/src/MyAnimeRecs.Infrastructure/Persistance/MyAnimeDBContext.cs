using Microsoft.EntityFrameworkCore;
using MyAnimeRecs.Domain;

namespace MyAnimeRecs.Infrastructure.Persistance
{
    public class MyAnimeDBContext : DbContext
    {
        public MyAnimeDBContext(DbContextOptions<MyAnimeDBContext> options) : base(options)
        {
        }

        // public DbSet<Anime> Animes { get; set; }
        // public DbSet<User> Users { get; set; }
        // public DbSet<Recommendation> Recommendations { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure entity properties and relationships here if needed
        }
    }
}