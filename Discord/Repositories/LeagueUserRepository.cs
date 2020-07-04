using Microsoft.EntityFrameworkCore;
using RugbyRoyale.Discord.App.Repository;
using RugbyRoyale.Discord.Context;
using RugbyRoyale.Entities.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RugbyRoyale.Discord.Repositories
{
    public class LeagueUserRepository : Repository, ILeagueUserRepository
    {
        public LeagueUserRepository(DataContext db) : base(db)
        {
        }

        public async Task<bool> SaveAsync(LeagueUser leagueUser)
        {
            try
            {
                db.Add(leagueUser);
                return (await db.SaveChangesAsync()) == 1;
            }
            catch (Exception e)
            {
                // TODO: LOG ERROR
                return false;
            }
        }

        public async Task<LeagueUser> GetAsync(Guid leagueID, string userID)
        {
            try
            {
                return await db.LeagueUsers.FirstOrDefaultAsync(lu => lu.LeagueID == leagueID && lu.UserID == userID);
            }
            catch (Exception e)
            {
                // TODO: LOG ERROR
                return null;
            }
        }

        public async Task<int> CountAsync(Guid leagueID)
        {
            try
            {
                return await db.LeagueUsers.Where(lu => lu.LeagueID == leagueID).CountAsync();
            }
            catch (Exception e)
            {
                // TODO: LOG ERROR
                return 0;
            }
        }
    }
}