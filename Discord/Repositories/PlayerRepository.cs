using RugbyRoyale.Discord.App.Repository;
using RugbyRoyale.Discord.Context;
using RugbyRoyale.Entities.Model;
using Serilog;
using System;
using System.Threading.Tasks;

namespace RugbyRoyale.Discord.Repositories
{
    public class PlayerRepository : Repository, IPlayerRepository
    {
        public PlayerRepository(DataContext db) : base(db)
        {
        }

        public async Task<bool> ExistsAsync(string playerID)
        {
            try
            {
                throw new NotImplementedException();
            }
            catch (Exception e)
            {
                Log.Error(e, "DB Error checking for Player with playerID {PlayerID}", playerID);
                return false;
            }
        }

        public async Task<bool> SaveAsync(Player player)
        {
            try
            {
                throw new NotImplementedException();
            }
            catch (Exception e)
            {
                Log.Error(e, "DB Error saving player: {@Player}", player);
                return false;
            }
        }
    }
}