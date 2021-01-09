using Microsoft.EntityFrameworkCore;
using RugbyRoyale.Discord.App.Repository;
using RugbyRoyale.Discord.Context;
using RugbyRoyale.Entities.Model;
using Serilog;
using System;
using System.Threading.Tasks;

namespace RugbyRoyale.Discord.Repositories
{
    public class TeamRepository : Repository, ITeamRepository
    {
        public TeamRepository(DataContext db) : base(db)
        {
        }

        public async Task<bool> ExistsAsync(string userID)
        {
            try
            {
                return await db.Teams.AnyAsync(t => t.UserID == userID);
            }
            catch (Exception e)
            {
                Log.Error(e, "DB Error checking for team with userID {UserID}", userID);
                return false;
            }
        }

        public async Task<Team> GetAsync(Guid teamID)
        {
            try
            {
                return await db.Teams.FirstOrDefaultAsync(t => t.TeamID == teamID);
            }
            catch (Exception e)
            {
                Log.Error(e, "DB Error getting team with teamID {TeamID}", teamID);
                return null;
            }
        }

        public async Task<Team> GetAsync(string userID)
        {
            try
            {
                return await db.Teams.FirstOrDefaultAsync(t => t.UserID == userID);
            }
            catch (Exception e)
            {
                Log.Error(e, "DB Error getting team with userID {UserID}", userID);
                return null;
            }
        }

        public async Task<bool> EditAsync(Team team)
        {
            try
            {
                db.Update(team);
                return (await db.SaveChangesAsync()) == 1;
            }
            catch (Exception e)
            {
                Log.Error(e, "DB Error editing team: {@Team}", team);
                return false;
            }
        }

        public async Task<bool> SaveAsync(Team team)
        {
            try
            {
                db.Add(team);
                return (await db.SaveChangesAsync()) == 1;
            }
            catch (Exception e)
            {
                Log.Error(e, "DB Error saving team: {@Team}", team);
                return false;
            }
        }
    }
}