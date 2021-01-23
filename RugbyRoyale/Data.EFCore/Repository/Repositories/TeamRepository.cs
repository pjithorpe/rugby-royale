using Microsoft.EntityFrameworkCore;
using RugbyRoyale.Data.EFCore.Context;
using RugbyRoyale.Data.Repository;
using RugbyRoyale.Entities.Model;
using Serilog;
using System;
using System.Threading.Tasks;

namespace RugbyRoyale.Data.EFCore.Repository
{
    public class TeamRepository : RepositoryBase<Team>, ITeamRepository
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
                Log.Error(e, "DB Error checking for team with UserID {UserID}", userID);
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
                Log.Error(e, "DB Error getting Team with TeamID {TeamID}", teamID);
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
                Log.Error(e, "DB Error getting Team with UserID {UserID}", userID);
                return null;
            }
        }
    }
}