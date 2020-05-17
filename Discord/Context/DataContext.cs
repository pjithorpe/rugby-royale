using Microsoft.EntityFrameworkCore;
using RugbyRoyale.Entities.Model;

namespace RugbyRoyale.Discord.Context
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }

        public DbSet<Player> Players { get; set; }
    }
}