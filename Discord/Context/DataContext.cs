using Microsoft.EntityFrameworkCore;
using RugbyRoyale.Entities.Model;

namespace RugbyRoyale.Discord.Context
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }

        public DbSet<League> Leagues { get; set; }
        public DbSet<MatchResult> MatchResult { get; set; }
        public DbSet<Player> Players { get; set; }
        public DbSet<Team> Teams { get; set; }
        public DbSet<Teamsheet> Teamsheets { get; set; }
        public DbSet<User> Users { get; set; }
    }
}