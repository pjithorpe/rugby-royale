using RugbyRoyale.Entities.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RugbyRoyale.Data.Repository
{
    public interface ILeagueRepository : IRepository<League>
    {
        /// <summary>
        /// Returns true if a league exists which the specified user owns, otherwise false.
        /// </summary>
        /// <param name="userID">The userID of the owner to search for.</param>
        /// <returns></returns>
        Task<bool> ExistsWithOwnerAsync(string userID);

        Task<League> GetAsync(Guid leagueID);

        Task<League> GetAsync(string userID);

        Task<League> GetWithUsersAsync(Guid leagueID);

        Task<League> GetWithUsersAsync(string userID);

        Task<List<League>> ListAllAsync();
    }
}