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
    public class UserRepository : RepositoryBase<User>, IUserRepository
    {
        public UserRepository(DataContext db) : base(db)
        {
        }

        public async Task<User> GetAsync(string userID)
        {
            try
            {
                return await db.Users.FindAsync(userID);
            }
            catch (Exception e)
            {
                Log.Error(e, "DB Error getting user with userID {UserID}", userID);
                return null;
            }
        }

        public async Task<User> GetWithTeamAsync(string userID)
        {
            try
            {
                return await db.Users.Include(u => u.Team).Include(u => u.Leagues).SingleOrDefaultAsync(u => u.UserID == userID);
            }
            catch (Exception e)
            {
                Log.Error(e, "DB Error getting user with userID {UserID}", userID);
                return null;
            }
        }

        public async Task<bool> ExistsAsync(string userID)
        {
            try
            {
                return await db.Users.AnyAsync(u => u.UserID == userID);
            }
            catch (Exception e)
            {
                Log.Error(e, "DB Error checking for user with userID {UserID}", userID);
                return false;
            }
        }

        public async Task<bool> AddAsync(User user)
        {
            try
            {
                db.Add(user);
                return (await db.SaveChangesAsync()) == 1;
            }
            catch (Exception e)
            {
                Log.Error(e, "DB Error saving user: {@User}", user);
                return false;
            }
        }

        public async Task<int> CountAsync(Guid leagueID)
        {
            try
            {
                return await db.Leagues.Include(l => l.Users).CountAsync(l => l.LeagueID == leagueID);
            }
            catch (Exception e)
            {
                Log.Error(e, "DB Error counting users for league: {@League}", leagueID);
                return 0;
            }
        }

        public async Task<List<User>> ListAsync(Guid leagueID)
        {
            try
            {
                return await db.Users.Where(u => u.Leagues.Any(l => l.LeagueID == leagueID)).ToListAsync();
            }
            catch (Exception e)
            {
                Log.Error(e, "DB Error getting league users for leagueID: {LeagueID}", leagueID);
                return null;
            }
        }
    }
}