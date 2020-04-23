using RugbyRoyale.App.Model;

namespace RugbyRoyale.Repositories
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