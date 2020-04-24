using RugbyRoyale.App;

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