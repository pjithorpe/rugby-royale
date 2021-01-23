namespace RugbyRoyale.Data.Repository
{
    public interface IRepository<T>
    {
        bool Add(T entity);
        bool Remove(T entity);
    }
}