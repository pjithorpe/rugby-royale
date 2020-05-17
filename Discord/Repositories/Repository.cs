using RugbyRoyale.Discord.Context;

namespace RugbyRoyale.Discord.Repositories
{
    public abstract class Repository
    {
        protected DataContext db;

        public Repository(DataContext db)
        {
            this.db = db;
        }
    }
}