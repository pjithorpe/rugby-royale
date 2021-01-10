using Microsoft.EntityFrameworkCore;
using RugbyRoyale.Discord.App.Repository;
using RugbyRoyale.Discord.Context;
using RugbyRoyale.Entities.Model;
using Serilog;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RugbyRoyale.Discord.Repositories
{
    public class LeagueRepository : Repository, ILeagueRepository
    {
        public LeagueRepository(DataContext db) : base(db)
        {
        }

        public async Task<bool> SaveAsync(League league)
        {
            try
            {
                db.Add(league);
                return (await db.SaveChangesAsync()) == 1;
            }
            catch (Exception e)
            {
                Log.Error(e, "DB Error saving league: {@League}", league);
                return false;
            }
        }

        public async Task<League> GetAsync(Guid leagueID)
        {
            try
            {
                return await db.Leagues.FirstOrDefaultAsync(l => l.LeagueID == leagueID);
            }
            catch (Exception e)
            {
                Log.Error(e, "DB Error getting League with leagueID {LeagueID}", leagueID);
                return null;
            }
        }

        public async Task<League> GetAsync(string userID)
        {
            try
            {
                return await db.Leagues.FirstOrDefaultAsync(l => l.UserID == userID);
            }
            catch (Exception e)
            {
                Log.Error(e, "DB Error getting League with userID {UserID}", userID);
                return null;
            }
        }

        public async Task<List<League>> ListAllAsync()
        {
            try
            {
                return await db.Leagues.ToListAsync();
            }
            catch (Exception e)
            {
                Log.Error(e, "DB Error getting all leagues");
                return null;
            }
        }
    }
}