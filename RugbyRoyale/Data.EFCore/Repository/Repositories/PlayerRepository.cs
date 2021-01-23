using RugbyRoyale.Data.EFCore.Context;
using RugbyRoyale.Data.Repository;
using RugbyRoyale.Entities.Model;
using Serilog;
using System;
using System.Threading.Tasks;

namespace RugbyRoyale.Data.EFCore.Repository
{
    public class PlayerRepository : RepositoryBase<Player>, IPlayerRepository
    {
        public PlayerRepository(DataContext db) : base(db)
        {
        }

        public async Task<bool> ExistsAsync(Guid playerID)
        {
            try
            {
                return await db.Players.FindAsync(playerID) != null;
            }
            catch (Exception e)
            {
                Log.Error(e, "DB Error checking for Player with PlayerID {PlayerID}", playerID);
                return false;
            }
        }
    }
}