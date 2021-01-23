using Microsoft.EntityFrameworkCore;
using RugbyRoyale.Data.EFCore.Context;
using RugbyRoyale.Data.Repository;
using RugbyRoyale.Entities.Model;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RugbyRoyale.Data.EFCore.Repository
{
    public class LeagueRepository : RepositoryBase<League>, ILeagueRepository
    {
        public LeagueRepository(DataContext db) : base(db)
        {
        }

        public async Task<League> GetAsync(Guid leagueID)
        {
            try
            {
                return await db.Leagues.FindAsync(leagueID);
            }
            catch (Exception e)
            {
                Log.Error(e, "DB Error getting League with LeagueID {LeagueID}", leagueID);
                return null;
            }
        }
        public async Task<League> GetWithUsersAsync(Guid leagueID)
        {
            try
            {
                return await db.Leagues.Include(l => l.Users).SingleOrDefaultAsync(l => l.LeagueID == leagueID);
            }
            catch (Exception e)
            {
                Log.Error(e, "DB Error getting League+Users with LeagueID {LeagueID}", leagueID);
                return null;
            }
        }


        public async Task<League> GetAsync(string userID)
        {
            try
            {
                return await db.Leagues.FirstOrDefaultAsync(l => l.OwnerID == userID);
            }
            catch (Exception e)
            {
                Log.Error(e, "DB Error getting League with UserID {UserID}", userID);
                return null;
            }
        }

        public async Task<bool> ExistsWithOwnerAsync(string userID)
        {
            try
            {
                return await db.Leagues.AnyAsync(l => l.OwnerID == userID);
            }
            catch (Exception e)
            {
                Log.Error(e, "DB Error checking for League with owner with UserID {UserID}", userID);
                return false;
            }
        }

        /// <summary>
        /// Gets the League entity with given OwnerID, and any related Users.
        /// </summary>
        /// <param name="ownerID">UserID of League owner.</param>
        /// <returns></returns>
        public async Task<League> GetWithUsersAsync(string ownerID)
        {
            try
            {
                return await db.Leagues.Include(l => l.Users).SingleOrDefaultAsync(l => l.OwnerID == ownerID);
            }
            catch (Exception e)
            {
                Log.Error(e, "DB Error getting League+Users with owner with UserID {UserID}", ownerID);
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