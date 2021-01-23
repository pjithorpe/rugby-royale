using RugbyRoyale.Data.EFCore.Context;
using RugbyRoyale.Data.Repository;
using Serilog;
using System;

namespace RugbyRoyale.Data.EFCore.Repository
{
    public abstract class RepositoryBase<T> : IRepository<T> where T : class
    {
        protected DataContext db;

        public RepositoryBase(DataContext db)
        {
            this.db = db;
        }

        public bool Add(T entity)
        {
            try
            {
                db.Add(entity);
                return true;
            }
            catch (Exception e)
            {
                Log.Error(e, "Error while adding {EntityType} entity to tracking: {@Entity}", entity.GetType().FullName, entity);
                return false;
            }
        }

        public bool Remove(T entity)
        {
            try
            {
                db.Remove(entity);
                return true;
            }
            catch (Exception e)
            {
                Log.Error(e, "Error while removing {EntityType} entity from tracking: {@Entity}", entity.GetType().FullName, entity);
                return false;
            }
        }
    }
}