using Microsoft.EntityFrameworkCore;
using RugbyRoyale.Discord.App.Repository;
using RugbyRoyale.Discord.Context;
using RugbyRoyale.Entities.Model;
using Serilog;
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
                Log.Error(e, "DB Error saving leagueUser: {@LeagueUser}", leagueUser);
                return false;
            }
        }

        public async Task<bool> ExistsAsync(string userID)
        {
            try
            {
                return await db.LeagueUsers.AnyAsync(lu => lu.UserID == userID);
            }
            catch (Exception e)
            {
                Log.Error(e, "DB Error checking for LeagueUser with userID {UserID}", userID);
                return false;
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
                Log.Error(e, "DB Error counting LeagueUsers with leagueID {LeagueID}", leagueID);
                return 0;
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
                Log.Error(e, "DB Error getting LeagueUser with leagueID {LeagueID} and userID {UserID}", leagueID, userID);
                return null;
            }
        }

        public async Task<List<LeagueUser>> ListAsync(string userID)
        {
            try
            {
                return await db.LeagueUsers.Where(lu => lu.UserID == userID).ToListAsync();
            }
            catch (Exception e)
            {
                Log.Error(e, "DB Error getting LeagueUsers with userID {UserID}", userID);
                return null;
            }
        }

        public async Task<List<LeagueUser>> ListAsync(Guid leagueID)
        {
            try
            {
                return await db.LeagueUsers.Where(lu => lu.LeagueID == leagueID).ToListAsync();
            }
            catch (Exception e)
            {
                Log.Error(e, "DB Error getting LeagueUsers with leagueID {LeagueID}", leagueID);
                return null;
            }
        }
    }
}