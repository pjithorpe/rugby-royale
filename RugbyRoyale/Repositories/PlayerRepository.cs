using RugbyRoyale.App.Model;
using RugbyRoyale.App.Repository;
using System;
using System.Threading.Tasks;

namespace RugbyRoyale.Repositories
{
    public class PlayerRepository : Repository, IPlayerRepository
    {
        public PlayerRepository(DataContext db) : base(db) { }

        public Task<bool> ExistsAsync(string playerId)
        {
            throw new NotImplementedException();
        }

        public Task SaveAsync(Player player)
        {
            throw new NotImplementedException();
        }
    }
}
