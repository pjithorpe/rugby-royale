using Microsoft.EntityFrameworkCore;
using RugbyRoyale.Model;

namespace RugbyRoyale.App
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }

        public DbSet<Player> Players { get; set; }
    }
}
