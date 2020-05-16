using RugbyRoyale.Client.Context;

namespace RugbyRoyale.Client.Repositories
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