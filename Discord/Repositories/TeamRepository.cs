using Microsoft.EntityFrameworkCore;
using RugbyRoyale.Discord.App.Repository;
using RugbyRoyale.Discord.Context;
using RugbyRoyale.Entities.Model;
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
                // TODO: LOG ERROR
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
                // TODO: LOG ERROR
                return null;
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
                // TODO: LOG ERROR
                return false;
            }
        }
    }
}