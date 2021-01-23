using RugbyRoyale.Entities.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RugbyRoyale.Data.Repository
{
    public interface IUserRepository : IRepository<User>
    {
        Task<User> GetAsync(string userID);

        Task<User> GetWithTeamAsync(string userID);

        Task<bool> ExistsAsync(string userID);

        Task<int> CountAsync(Guid leagueID);

        Task<List<User>> ListAsync(Guid leagueID);
    }
}