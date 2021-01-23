using RugbyRoyale.Entities.Model;
using System;
using System.Threading.Tasks;

namespace RugbyRoyale.Data.Repository
{
    public interface IPlayerRepository : IRepository<Player>
    {
        Task<bool> ExistsAsync(Guid playerID);
    }
}