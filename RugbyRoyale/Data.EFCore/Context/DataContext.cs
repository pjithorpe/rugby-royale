using Microsoft.EntityFrameworkCore;
using RugbyRoyale.Entities.Model;

namespace RugbyRoyale.Data.EFCore.Context
{
    public class DataContext : DbContext
    {
        /*
         * Parameterless constructor for Design time usage only
         */
        public DataContext()
        {
        }

        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }

        public DbSet<League> Leagues { get; set; }
        public DbSet<MatchResult> MatchResult { get; set; }
        public DbSet<Player> Players { get; set; }
        public DbSet<Team> Teams { get; set; }
        public DbSet<Teamsheet> Teamsheets { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Relationships

            // M-to-M

            // 1-to-M

            // 1-to-1
            modelBuilder.Entity<User>()
                .HasOne(u => u.OwnedLeague)
                .WithOne(l => l.Owner)
                .HasForeignKey<League>(u => u.OwnerID);

            // 0-to-1

            base.OnModelCreating(modelBuilder);
        }
    }
}