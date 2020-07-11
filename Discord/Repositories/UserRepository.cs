using Microsoft.EntityFrameworkCore;
using RugbyRoyale.Discord.App.Repository;
using RugbyRoyale.Discord.Context;
using RugbyRoyale.Entities.Model;
using System;
using System.Threading.Tasks;

namespace RugbyRoyale.Discord.Repositories
{
    public class UserRepository : Repository, IUserRepository
    {
        public UserRepository(DataContext db) : base(db)
        {
        }

        public async Task<User> GetAsync(string userID)
        {
            try
            {
                return await db.Users.FirstOrDefaultAsync(u => u.UserID == userID);
            }
            catch (Exception e)
            {
                // TODO: LOG ERROR
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
                // TODO: LOG ERROR
                return false;
            }
        }

        public async Task<bool> SaveAsync(User user)
        {
            try
            {
                db.Add(user);
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