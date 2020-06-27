using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System;

namespace RugbyRoyale.Discord.Context
{
    public class DataContextFactory : IDesignTimeDbContextFactory<DataContext>
    {
        public DataContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<DataContext>();
            optionsBuilder.UseSqlite($"Data Source={Environment.CurrentDirectory}/players.db");

            return new DataContext(optionsBuilder.Options);
        }
    }
}