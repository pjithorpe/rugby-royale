using Microsoft.EntityFrameworkCore;
using RugbyRoyale.App.Model;

namespace RugbyRoyale.Model
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }
        
        public DbSet<Player> Players { get; set; }
    }
}
